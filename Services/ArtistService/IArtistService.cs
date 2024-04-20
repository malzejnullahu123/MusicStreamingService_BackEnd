using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.ArtistService;

public interface IArtistService
{
    Task<ArtistResponseModel> CreateArtist(ArtistRequestModel createArtist);
    Task<List<ArtistResponseModel>> GetAllArtists();
    Task<ArtistResponseModel> FindById(int id);
    Task<ArtistResponseModel> DeleteById(int id);
}