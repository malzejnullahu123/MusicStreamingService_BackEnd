using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _dbContext;

        public GenreService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GenreResponseModel> FindById(int id)
        {
            var genre = await _dbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                throw new ArgumentException($"Genre with ID {id} not found.");
            }

            return new GenreResponseModel
            {
                GenreId = genre.GenreId,
                Name = genre.Name
            };
        }

        public async Task<List<GenreResponseModel>> GetAll()
        {
            var genres = await _dbContext.Genres.ToListAsync();
            return genres.Select(genre => new GenreResponseModel
            {
                GenreId = genre.GenreId,
                Name = genre.Name
            }).ToList();
        }

        public async Task<GenreResponseModel> CreateGenre(GenreRequestModel request)
        {
            var genre = new Genre
            {
                Name = request.Name
            };

            _dbContext.Genres.Add(genre);
            await _dbContext.SaveChangesAsync();

            return new GenreResponseModel
            {
                GenreId = genre.GenreId,
                Name = genre.Name
            };
        }

        public async Task<GenreResponseModel> DeleteById(int id)
        {
            var genre = await _dbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                throw new ArgumentException($"Genre with ID {id} not found.");
            }

            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();

            return new GenreResponseModel
            {
                GenreId = genre.GenreId,
                Name = genre.Name
            };
        }
    }
}
