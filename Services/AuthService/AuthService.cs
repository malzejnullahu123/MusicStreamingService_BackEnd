using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;

    public AuthService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Authenticate(LoginRequestModel request)
    {
        var user = _dbContext.Users
            .Where(user => user.Email == request.Email)
            .FirstOrDefault();
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        string hashedPassword = HashPassword(request.Password);
        if (user.Password == hashedPassword)
        {
            return TokenService.GenerateToken(user.UserId, user.Email, user.Role);
        }
        throw new ArgumentException("Invalid password");
    }

    public async Task<UserResponseModel> Me(string token)
    {
        var principal = TokenService.VerifyToken(token);
        
        var idClaim = principal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        if (idClaim == null || !int.TryParse(idClaim.Value, out var id))
        {
            throw new ArgumentException("UnAuthorized");
        }
        
        var user = _dbContext.Users.FirstOrDefault(user => user.UserId == id);

        if (user == null)
        {
            throw new ArgumentException("UnAuthorized3 su gjet");
        }
        
        return new UserResponseModel
        {
            UserId = user!.UserId,
            Email = user.Email,
            EmbedImgLink = user.EmbedImgLink,
            FullName = user.FullName,
            Username = user.Username
        };
    }
    
    

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

}