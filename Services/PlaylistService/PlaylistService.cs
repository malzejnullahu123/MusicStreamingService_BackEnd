using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.PlaylistService
{
    public class PlaylistService : IPlaylistService
    {
        private readonly AppDbContext _dbContext;

        public PlaylistService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlaylistResponseModel> CreatePlaylist(PlaylistRequestModel request)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User with the specified ID does not exist in the database.");
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

        public async Task<List<PlaylistResponseModel>> GetAllVisible(int pageNumber, int pageSize)
        {
            var playlists = await _dbContext.Playlists
                .OrderByDescending(p => p.PlaylistId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var playlistResponseModels = new List<PlaylistResponseModel>();
            foreach (var playlist in playlists)
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
        
        
        
    }
}
