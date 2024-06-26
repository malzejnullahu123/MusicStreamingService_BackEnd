using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.PlayHistoryService;

public interface IPlayHistoryService
{
    Task<List<PlayHistoryResponseModel>> GetPlayHistoryByUserId(string token);
    Task AddPlayHistory(string token, int songId, DateTime datePlayed);
}