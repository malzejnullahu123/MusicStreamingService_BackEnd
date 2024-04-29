using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly AppDbContext _dbContext;
        private readonly ExtractFromToken _extractor;

        public AlbumService(AppDbContext appDbContext, ExtractFromToken extractor)
        {
            _dbContext = appDbContext;
            _extractor = extractor;
        }

        public async Task<AlbumResponseModel> FindById(int id)
        {
            var album = await _dbContext.Albums.FindAsync(id);
            if (album == null)
            {
                throw new ArgumentException($"Album with ID {id} not found.");
            }

            var artist = await _dbContext.Artists.FindAsync(album.ArtistId);
            if (artist == null)
            {
                throw new InvalidOperationException("Artist not found for the album.");
            }

            return new AlbumResponseModel
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId,
                ArtistName = artist.Name, // Include the artist's name
                Image = album.Image
            };
        }


        /// <summary>
        /// //////////////////////aspak metode e mire, gjithsesi punon...
        /// </summary>
        /// <returns></returns>
        public async Task<List<AlbumResponseModel>> GetAll(int pageNumber, int pageSize)
        {
            var albums = await _dbContext.Albums
                .OrderByDescending(a => a.AlbumId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var albumResponseModels = new List<AlbumResponseModel>();
            foreach (var album in albums)
            {
                var artist = await _dbContext.Artists.FindAsync(album.ArtistId);
                if (artist != null)
                {
                    albumResponseModels.Add(new AlbumResponseModel
                    {
                        AlbumId = album.AlbumId,
                        Title = album.Title,
                        ReleaseDate = album.ReleaseDate,
                        ArtistId = album.ArtistId,
                        ArtistName = artist.Name,
                        Image = album.Image
                    });
                }
            }
            return albumResponseModels;
        }


        public async Task<AlbumResponseModel> CreateAlbum(string token, AlbumRequestModel request)
        {
            var artistId = _extractor.Id(token);
            var artist = await _dbContext.Artists.FirstOrDefaultAsync(a => a.ArtistId == artistId);
            if (artist == null)
            {
                throw new InvalidOperationException("Artist with the specified ID does not exist in the database.");
            }

            var album = new Album
            {
                Title = request.Title,
                ArtistId = artistId,
                ReleaseDate = DateTime.UtcNow,
                Image = request.Image
            };

            _dbContext.Albums.Add(album);
            await _dbContext.SaveChangesAsync();

            return new AlbumResponseModel
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId,
                ArtistName = artist.Name, // Include the artist's name
                Image = album.Image
            };
        }


        public async Task<AlbumResponseModel> DeleteById(int id)
        {
            var album = await _dbContext.Albums.FindAsync(id);
            if (album == null)
            {
                throw new ArgumentException($"Album with ID {id} not found.");
            }

            _dbContext.Albums.Remove(album);
            await _dbContext.SaveChangesAsync();

            return new AlbumResponseModel
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId,
                Image = album.Image,
            };
        }

        public async Task<AlbumResponseModel> AddSongToAlbum(int albumId, int songId)
        {
            var album = await _dbContext.Albums.Include(a => a.Songs).FirstOrDefaultAsync(a => a.AlbumId == albumId);
            if (album == null)
            {
                throw new ArgumentException($"Album with ID {albumId} not found.");
            }

            var song = await _dbContext.Songs.FirstOrDefaultAsync(s => s.SongId == songId);
            if (song == null)
            {
                throw new ArgumentException($"Song with ID {songId} not found.");
            }

            if (album.Songs.Any(s => s.SongId == songId))
            {
                throw new InvalidOperationException($"Song with ID {songId} already exists in album with ID {albumId}.");
            }

            album.Songs.Add(song);
            await _dbContext.SaveChangesAsync();

            return new AlbumResponseModel
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId,
                Image = album.Image
            };
        }



        
    }
}
