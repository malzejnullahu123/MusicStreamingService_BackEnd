using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.PlaylistService;

public interface IPlaylistService
{
    Task<PlaylistResponseModel> FindById(int id);
    Task<List<PlaylistResponseModel>> GetAllVisible(string token, int pageNumber, int pageSize);
    Task<PlaylistResponseModel> CreatePlaylist(string token, PlaylistRequestModel request);
    Task<PlaylistResponseModel> DeleteById(int id);
    
    Task<List<SongResponseModel>> GetSongsInPlaylist(int playlistId);
    Task AddSongToPlaylist(int playlistId, int songId);
    Task RemoveSongFromPlaylist(int playlistId, int songId);
    Task<List<PlaylistResponseModel>> GetPlaylistsOfUser(int userId, int pageNumber, int pageSize);
    Task<List<PlaylistResponseModel>> GetMyPlaylists(string token, int pageNumber, int pageSize);

}