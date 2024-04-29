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

        public async Task<PlaylistResponseModel> CreatePlaylist(string token, PlaylistRequestModel request)
        {
            var userId = _extractor.Id(token);
            var user = await _dbContext.Users.FindAsync(userId);

            if (user.UserId == null)
            {
                throw new ArgumentException("useri nuk egziston");
            }

            var playlist = new Playlist
            {
                Name = request.Name,
                UserId = userId,
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
            var allPlaylists = await _dbContext.Playlists
                .OrderByDescending(p => p.PlaylistId)
                .ToListAsync();

            if (token == null)
            {
                // Return only visible playlists if no token
                var playlistResponseModels = new List<PlaylistResponseModel>();
                foreach (var playlist in allPlaylists.Where(p => p.IsVisible)) // Filter visible playlists
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
                .Select(f => f.FollowingUserID)
                .ToListAsync();

            // Combine all playlists, including private ones from followed users
            var combinedPlaylists = allPlaylists
                .Where(p => p.IsVisible || followingUserIds.Contains(p.UserId) || p.UserId == userId) // Filter visible or followed user's playlists
                .OrderByDescending(p => p.PlaylistId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var playlistPersonalResponseModels = combinedPlaylists.Select(p => new PlaylistResponseModel
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
                EmbedIMGLink = song.EmbedIMGLink,
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
            int skip = (pageNumber - 1) * pageSize;

            var playlists = await _dbContext.Playlists
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PlaylistId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            if (!playlists.Any())
            {
                throw new ArgumentException("There are no playlists for the specified user.");
            }

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
        
        
        public async Task<List<PlaylistResponseModel>> GetMyPlaylists(string token, int pageNumber, int pageSize)
        {
            var userId = _extractor.Id(token);
            int skip = (pageNumber - 1) * pageSize;

            var playlists = await _dbContext.Playlists
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PlaylistId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            if (!playlists.Any())
            {
                throw new ArgumentException("There are no playlists for the specified user.");
            }

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
