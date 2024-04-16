using MusicStreamingService_BackEnd.Dto;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.ArtistService;

public interface IArtistService
{
    Task<Artist> CreateArtist(ArtistDto createArtist);
    Task<List<Artist>> GetAllArtists();
    Task<Artist> FindById(int id);
    Task<Artist> DeleteById(int id);
}