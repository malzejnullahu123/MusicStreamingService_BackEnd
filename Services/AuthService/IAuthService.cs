using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.AuthService;

public interface IAuthService
{
    Task<string> Authenticate(LoginRequestModel request);
    
    Task<UserResponseModel> Me(string token);

}