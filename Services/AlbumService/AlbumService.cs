using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly AppDbContext _dbContext;

        public AlbumService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<AlbumResponseModel> FindById(int id)
        {
            var album = await _dbContext.Albums.FindAsync(id);
            if (album == null)
            {
                throw new ArgumentException($"Album with ID {id} not found.");
            }

            return new AlbumResponseModel
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId,
                Image = album.Image
            };
        }

        public async Task<List<AlbumResponseModel>> GetAll()
        {
            var albums = await _dbContext.Albums.ToListAsync();
            return albums.Select(album => new AlbumResponseModel
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId,
                Image = album.Image
            }).ToList();
        }

        public async Task<AlbumResponseModel> CreateAlbum(AlbumRequestModel request)
        {
            var artist = await _dbContext.Artists.FirstOrDefaultAsync(a => a.ArtistId == request.ArtistId);
            if (artist == null)
            {
                throw new InvalidOperationException("Artist with the specified ID does not exist in the database.");
            }

            var album = new Album
            {
                Title = request.Title,
                ArtistId = request.ArtistId,
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
    }
}
