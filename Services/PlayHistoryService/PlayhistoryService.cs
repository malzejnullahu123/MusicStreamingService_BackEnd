using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.PlayHistoryService
{
    public class PlayHistoryService : IPlayHistoryService
    {
        private readonly AppDbContext _dbContext;

        public PlayHistoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PlayHistoryResponseModel>> GetPlayHistoryByUserId(string token)
        {
            var principal = TokenService.VerifyToken(token);
    
            var idClaim = principal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int.TryParse(idClaim.Value, out var id);
            
            var playHistoryEntries = await _dbContext.PlayHistories
                .Include(ph => ph.Song)
                .Where(ph => ph.UserId == id)
                .OrderByDescending(ph => ph.DatePlayed)
                .ToListAsync();

            return playHistoryEntries.Select(ph => new PlayHistoryResponseModel
            {
                PlayHistoryId = ph.PlayHistoryId,
                UserId = ph.UserId,
                SongId = ph.SongId,
                DatePlayed = ph.DatePlayed,
                Song = new SongResponseModel
                {
                    SongId = ph.Song.SongId,
                    Title = ph.Song.Title,
                    ArtistId = ph.Song.ArtistId,
                    GenreId = ph.Song.GenreId,
                    EmbedLink = ph.Song.EmbedLink,
                    EmbedIMGLink = ph.Song.EmbedIMGLink,
                }
            }).ToList();
        }

        public async Task AddPlayHistory(int userId, int songId, DateTime datePlayed)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} not found.");
            }

            var song = await _dbContext.Songs.FindAsync(songId);
            if (song == null)
            {
                throw new ArgumentException($"Song with ID {songId} not found.");
            }

            var playHistory = new PlayHistory
            {
                UserId = userId,
                SongId = songId,
                DatePlayed = datePlayed
            };

            _dbContext.PlayHistories.Add(playHistory);
            await _dbContext.SaveChangesAsync();
        }
    }
}