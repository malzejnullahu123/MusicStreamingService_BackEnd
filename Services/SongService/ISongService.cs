using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.SongService;

public interface ISongService
{
    Task<SongResponseModel> FindById(int id);
    Task<List<SongResponseModel>> GetAll();
    Task<SongResponseModel> CreateSong(SongRequestModel request);
    Task<SongResponseModel> DeleteById(int id);
    Task<List<SongResponseModel>> GetSongsByGenre(int genreId);
}