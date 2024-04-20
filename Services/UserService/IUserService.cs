using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services;

public interface IUserService
{
    Task<UserResponseModel> CreateUser(UserRequestModel createUser);
    Task<List<UserResponseModel>> GetAllUsers();
    Task<User> EditUser(int id, EditUserRequestModel editUser);

    Task<UserResponseModel> FindById(int id);
    Task<UserResponseModel> DeleteById(int id);
}