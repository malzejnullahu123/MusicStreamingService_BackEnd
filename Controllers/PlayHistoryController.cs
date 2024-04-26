using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.PlayHistoryService;

namespace MusicStreamingService_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayHistoryController : ControllerBase
    {
        private readonly IPlayHistoryService _playHistoryService;

        public PlayHistoryController(IPlayHistoryService playHistoryService)
        {
            _playHistoryService = playHistoryService;
        }

        [HttpGet("{token}")]
        // [Authorize]
        public async Task<ActionResult<List<PlayHistoryResponseModel>>> GetPlayHistory()
        {
            // string token =
            //     "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzIiwiZW1haWwiOiJhc2RAYXNkLmFzZCIsInJvbGUiOiJ1c2VyIiwibmJmIjoxNzE0MDgzNTMzLCJleHAiOjE3MTQyNTYzMzMsImlhdCI6MTcxNDA4MzUzM30.pCQ9yOP30CSMWdITEc7dkr8s-4nZYwTFaylWDd6JDN8";  // HttpContext.Request.Headers["Authorization"];

            string token = HttpContext.Request.Headers["Authorization"];

            var playHistory = await _playHistoryService.GetPlayHistoryByUserId(token);
            if (playHistory.Count == 0)
            {
                return NotFound("No play history found for the user.");
            }
            return Ok(playHistory);
        }


        [HttpPost("listen")]
        // [Authorize]
        public async Task<ActionResult> AddPlayHistory(PlayHistoryRequestModel request)
        {

            string token = HttpContext.Request.Headers["Authorization"];

            try
            {
                await _playHistoryService.AddPlayHistory(token, request.SongId, DateTime.UtcNow);
                return Ok("Listened...");
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}