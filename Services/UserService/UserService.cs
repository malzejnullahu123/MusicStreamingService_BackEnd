using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.UserService;

public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
//
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

            List<string> links = new List<string>
            {
                "https://cdn-icons-png.flaticon.com/512/6596/6596121.png",
                "https://cdn-icons-png.flaticon.com/512/3607/3607444.png",
                "https://cdn2.iconfinder.com/data/icons/audio-16/96/user_avatar_profile_login_button_account_member-512.png",
                "https://cdn-icons-png.flaticon.com/512/3541/3541871.png",
                "https://cdn4.iconfinder.com/data/icons/social-messaging-ui-color-and-shapes-3/177800/129-512.png",
                "https://www.pikpng.com/pngl/m/80-805068_my-profile-icon-blank-profile-picture-circle-clipart.png"
            };

            Random random = new Random();
            int randomIndex = random.Next(0, links.Count);
            string randomLink = links[randomIndex];
            
            var user = new User
            {
                FullName = request.FullName,
                Username = request.Username,
                Email = request.Email,
                Password = HashPassword(request.Password),
                EmbedImgLink = randomLink,
                Role = "user"
            };
            
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            
            var response = new UserResponseModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                EmbedImgLink = user.EmbedImgLink,
                Email = user.Email
            };
            return response;
        }

        public async Task<List<UserResponseModel>> GetAllUsers(int pageNumber, int pageSize)
        {
            var users = await _dbContext.Users
                .OrderByDescending(user => user.UserId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users.Select(user => new UserResponseModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                EmbedImgLink = user.EmbedImgLink,
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
            user.Password = HashPassword(request.Password);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            var response = new User()
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                EmbedImgLink = user.EmbedImgLink,
                Password = request.Password
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
                EmbedImgLink = user.EmbedImgLink,
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

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }