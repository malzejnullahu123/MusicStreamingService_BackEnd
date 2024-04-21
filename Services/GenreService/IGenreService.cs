using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.GenreService
{
    public interface IGenreService
    {
        Task<GenreResponseModel> FindById(int id);
        Task<List<GenreResponseModel>> GetAll();
        Task<GenreResponseModel> CreateGenre(GenreRequestModel request);
        Task<GenreResponseModel> DeleteById(int id);
    }
}