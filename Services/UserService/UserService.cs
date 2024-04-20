using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.UserService;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }
    
    public async Task<UserResponseModel> CreateUser([FromBody] UserRequestModel request)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }        
        var user = new User
        {
            FullName = request.FullName,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var response = new UserResponseModel
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        };
        return response;
    }

    public async Task<List<UserResponseModel>> GetAllUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return users.Select(user => new UserResponseModel
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        }).ToList();
    }

    public async Task<User> EditUser(int id, EditUserRequestModel request)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {id} not found.");
        }
        user.FullName = request.FullName;
        user.Username = request.Username;
        user.Password = request.Password;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        var response = new User()
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password
        };

        return response;
    }

    public async Task<UserResponseModel> FindById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }
        
        return new UserResponseModel
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<UserResponseModel> DeleteById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return new UserResponseModel
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        };
    }
    
    
    
}