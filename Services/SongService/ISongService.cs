using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.SongService;

public interface ISongService
{
    Task<SongResponseModel> FindById(int id);
    Task<List<SongResponseModel>> GetNew(int pageSize);
    Task<List<SongResponseModel>> GetAll(int pageNumber, int pageSize);
    Task<SongResponseModel> CreateSong(SongRequestModel request);
    Task<SongResponseModel> DeleteById(int id);
    Task<List<SongResponseModel>> GetSongsByGenre(int genreId);
    Task<List<SongResponseModel>> GetSongsByArtist(int artistId);
    Task<List<SongResponseModel>> GetSongsByAlbum(int albumId);
    Task<List<SongResponseModel>> GetRecommendedSongs(string token);

}