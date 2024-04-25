using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.ArtistService;

public interface IArtistService
{
    Task<ArtistResponseModel> CreateArtist([FromQuery] string token, ArtistRequestModel createArtist);
    Task<List<ArtistResponseModel>> GetAllArtists(int pageNumber, int pageSize);
    Task<ArtistResponseModel> FindById(int id);
    Task<ArtistResponseModel> DeleteById(int id);
}