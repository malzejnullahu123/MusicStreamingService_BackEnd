using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Dto;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.UserService;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }
    
    public async Task<User> CreateUser([FromBody] SignUpDto request)
    {
        //Kontrollo nese useri egziston permes email
        
        var user = new User
        {
            Id = _dbContext.Users.Count() + 1,
            FullName = request.Password,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<List<GetUserDto>> GetAllUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return users.Select(user => new GetUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        }).ToList();
    }

    public async Task<GetUserDto> FindById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }
        
        return new GetUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<GetUserDto> DeleteById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return new GetUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        };
    }
    
    
    
}