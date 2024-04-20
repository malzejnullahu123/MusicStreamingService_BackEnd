using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.AlbumService;

public interface IAlbumService
{
    Task<Album> FindById(int id);
    Task<List<Album>> GetAll();
    Task <AlbumResponseModel> CreateAlbum(AlbumRequestModel album);
    Task<Album> DeleteById(int id);


}