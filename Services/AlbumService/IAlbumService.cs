using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.AlbumService;

public interface IAlbumService
{
    Task<AlbumResponseModel> FindById(int id);
    Task<List<AlbumResponseModel>> GetAll();
    Task <AlbumResponseModel> CreateAlbum(AlbumRequestModel album);
    Task<AlbumResponseModel> DeleteById(int id);


}