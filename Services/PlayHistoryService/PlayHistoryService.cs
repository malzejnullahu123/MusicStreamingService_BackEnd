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
        private readonly ExtractFromToken _extractor;

        public PlayHistoryService(AppDbContext dbContext, ExtractFromToken extractor)
        {
            _dbContext = dbContext;
            _extractor = extractor;
        }

        public async Task<List<PlayHistoryResponseModel>> GetPlayHistoryByUserId(string token)
        {
            var id = _extractor.Id(token);
            
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

        public async Task AddPlayHistory(string token, int songId, DateTime datePlayed)
        {
            var userId = _extractor.Id(token);

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