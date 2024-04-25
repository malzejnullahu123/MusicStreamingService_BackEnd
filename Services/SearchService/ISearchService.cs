using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.SearchService;

public interface ISearchService
{
    Task<List<SongResponseModel>> SearchSongs(string query, int pageNumber, int pageSize);
    Task<List<ArtistResponseModel>> SearchArtists(string query, int pageNumber, int pageSize);
    Task<List<AlbumResponseModel>> SearchAlbums(string query, int pageNumber, int pageSize);
}