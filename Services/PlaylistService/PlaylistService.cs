using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.PlaylistService
{
    public class PlaylistService : IPlaylistService
    {
        private readonly AppDbContext _dbContext;
        private readonly ExtractFromToken _extractor;

        public PlaylistService(AppDbContext dbContext, ExtractFromToken extractor)
        {
            _dbContext = dbContext;
            _extractor = extractor;
        }

        public async Task<PlaylistResponseModel> CreatePlaylist(PlaylistRequestModel request)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);

            if (user.UserId == null)
            {
                throw new ArgumentException("useri nuk egziston");
            }

            var playlist = new Playlist
            {
                Name = request.Name,
                UserId = request.UserId,
                Image = request.Image,
                IsVisible = request.IsVisible
            };

            _dbContext.Playlists.Add(playlist);
            await _dbContext.SaveChangesAsync();

            return new PlaylistResponseModel
            {
                PlaylistId = playlist.PlaylistId,
                Name = playlist.Name,
                UserId = playlist.UserId,
                Image = playlist.Image,
                IsVisible = request.IsVisible
            };
        }

        public async Task<PlaylistResponseModel> FindById(int id)
        {
            var playlist = await _dbContext.Playlists.FindAsync(id);
            if (playlist == null)
            {
                throw new ArgumentException($"Playlist with ID {id} not found.");
            }

            return new PlaylistResponseModel
            {
                PlaylistId = playlist.PlaylistId,
                Name = playlist.Name,
                UserId = playlist.UserId,
                Image = playlist.Image,
                IsVisible = playlist.IsVisible
            };
        }

        public async Task<List<PlaylistResponseModel>> GetAllVisible(string token, int pageNumber, int pageSize)
        {
            var visiblePlaylists = await _dbContext.Playlists
                .Where(p => p.IsVisible) // Filter by IsVisible
                .OrderByDescending(p => p.PlaylistId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            if (token == null)
            {
                var playlistResponseModels = new List<PlaylistResponseModel>();
                foreach (var playlist in visiblePlaylists)
                {
                    playlistResponseModels.Add(new PlaylistResponseModel
                    {
                        PlaylistId = playlist.PlaylistId,
                        Name = playlist.Name,
                        UserId = playlist.UserId,
                        Image = playlist.Image,
                        IsVisible = playlist.IsVisible
                    });
                }

                return playlistResponseModels;
            }

            var userId = _extractor.Id(token);
            var followingUserIds = await _dbContext.Follows
                .Where(f => f.UserID == userId)
                .Select(f => f.FollowId)
                .ToListAsync();
            var followingPlaylists = await _dbContext.Playlists
                .Where(p => followingUserIds.Contains(p.UserId))
                .ToListAsync();
            
            var allPlaylists = visiblePlaylists.Concat(followingPlaylists)
                .OrderByDescending(p => p.PlaylistId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var playlistPersonalResponseModels = allPlaylists.Select(p => new PlaylistResponseModel
            {
                PlaylistId = p.PlaylistId,
                Name = p.Name,
                UserId = p.UserId,
                Image = p.Image,
                IsVisible = p.IsVisible
            }).ToList();

            return playlistPersonalResponseModels;
        }


        public async Task<PlaylistResponseModel> DeleteById(int id)
        {
            var playlist = await _dbContext.Playlists.FindAsync(id);
            if (playlist == null)
            {
                throw new ArgumentException($"Playlist with ID {id} not found.");
            }

            _dbContext.Playlists.Remove(playlist);
            await _dbContext.SaveChangesAsync();

            return new PlaylistResponseModel
            {
                PlaylistId = playlist.PlaylistId,
                Name = playlist.Name,
                UserId = playlist.UserId,
                Image = playlist.Image,
                IsVisible = playlist.IsVisible
            };
        }
        
        
        
        
        //crd for songs on playlist
        public async Task<List<SongResponseModel>> GetSongsInPlaylist(int playlistId)
        {
            var playlistExists = await _dbContext.Playlists.AnyAsync(p => p.PlaylistId == playlistId);
            if (!playlistExists)
            {
                throw new ArgumentException($"Playlist with ID {playlistId} not found.");
            }

            var playlistSongs = await _dbContext.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .Select(ps => ps.Song)
                .ToListAsync();

            return playlistSongs.Select(song => new SongResponseModel
            {
                SongId = song.SongId,
                Title = song.Title,
                ArtistId = song.ArtistId,
                GenreId = song.GenreId,
                EmbedLink = song.EmbedLink
            }).ToList();
        }


        public async Task AddSongToPlaylist(int playlistId, int songId)
        {
            var playlist = await _dbContext.Playlists.FindAsync(playlistId);
            if (playlist == null)
            {
                throw new ArgumentException($"Playlist with ID {playlistId} not found.");
            }

            var song = await _dbContext.Songs.FindAsync(songId);
            if (song == null)
            {
                throw new ArgumentException($"Song with ID {songId} not found.");
            }

            var existingPlaylistSong = await _dbContext.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (existingPlaylistSong != null)
            {
                throw new InvalidOperationException("Song is already in the playlist.");
            }

            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId
            };

            _dbContext.PlaylistSongs.Add(playlistSong);
            await _dbContext.SaveChangesAsync();
        }



        public async Task RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var playlist = await _dbContext.Playlists.FindAsync(playlistId);
            if (playlist == null)
            {
                throw new ArgumentException($"Playlist with ID {playlistId} not found.");
            }

            var song = await _dbContext.Songs.FindAsync(songId);
            if (song == null)
            {
                throw new ArgumentException($"Song with ID {songId} not found.");
            }

            var playlistSong = await _dbContext.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (playlistSong != null)
            {
                _dbContext.PlaylistSongs.Remove(playlistSong);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Song not found in the playlist.");
            }
        }

        public async Task<List<PlaylistResponseModel>> GetPlaylistsOfUser(int userId, int pageNumber, int pageSize)
        {
            // Calculate the number of items to skip
            int skip = (pageNumber - 1) * pageSize;

            // Query the database for playlists created by the user, applying pagination
            var playlists = await _dbContext.Playlists
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PlaylistId) // Assuming there's a CreatedAt property to order by
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            if (!playlists.Any())
            {
                throw new ArgumentException("There are no playlists for the specified user.");
            }

            // Map the playlists to PlaylistResponseModel objects
            var playlistResponseModels = playlists.Select(playlist => new PlaylistResponseModel
            {
                PlaylistId = playlist.PlaylistId,
                Name = playlist.Name,
                UserId = playlist.UserId,
                Image = playlist.Image,
                IsVisible = playlist.IsVisible
            }).ToList();

            return playlistResponseModels;
        }

    }
}
