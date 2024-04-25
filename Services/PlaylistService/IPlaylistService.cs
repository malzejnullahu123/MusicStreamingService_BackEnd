using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.PlaylistService;

public interface IPlaylistService
{
    Task<PlaylistResponseModel> FindById(int id);
    Task<List<PlaylistResponseModel>> GetAllVisible(int pageNumber, int pageSize);
    Task<PlaylistResponseModel> CreatePlaylist(PlaylistRequestModel request);
    Task<PlaylistResponseModel> DeleteById(int id);
    
    Task<List<SongResponseModel>> GetSongsInPlaylist(int playlistId);
    Task AddSongToPlaylist(int playlistId, int songId);
    Task RemoveSongFromPlaylist(int playlistId, int songId);
}