using MusicStreamingService_BackEnd.Dto;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services;

public interface IUserService
{
    Task<User> CreateUser(SignUpDto createUser);
    Task<List<GetUserDto>> GetAllUsers();
    Task<GetUserDto> FindById(int id);
    Task<GetUserDto> DeleteById(int id);
}