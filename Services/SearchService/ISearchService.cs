using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.SearchService;

public interface ISearchService
{
    Task<List<SongResponseModel>> SearchSongs(string query, int pageNumber, int pageSize);
    Task<List<ArtistResponseModel>> SearchArtists(string query, int pageNumber, int pageSize);
    Task<List<AlbumResponseModel>> SearchAlbums(string query, int pageNumber, int pageSize);
    Task<List<PlaylistResponseModel>> SearchPublicPlaylist(string query, int pageNumber, int pageSize);
    Task<List<UserResponseModel>> SearchUsers(string query, int pageNumber, int pageSize);

}