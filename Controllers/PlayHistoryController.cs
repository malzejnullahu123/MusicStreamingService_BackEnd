using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.PlayHistoryService;

namespace MusicStreamingService_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayHistoryController : ControllerBase
    {
        private readonly IPlayHistoryService _playHistoryService;

        public PlayHistoryController(IPlayHistoryService playHistoryService)
        {
            _playHistoryService = playHistoryService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<PlayHistoryResponseModel>>> GetPlayHistory(int userId)
        {
            var playHistory = await _playHistoryService.GetPlayHistoryByUserId(userId);
            if (playHistory.Count == 0)
            {
                return NotFound("No play history found for the user.");
            }
            return Ok(playHistory);
        }


        [HttpPost]
        public async Task<ActionResult> AddPlayHistory(PlayHistoryRequestModel request)
        {
            try
            {
                await _playHistoryService.AddPlayHistory(request.UserId, request.SongId, DateTime.UtcNow);
                return Ok("Listened...");
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}