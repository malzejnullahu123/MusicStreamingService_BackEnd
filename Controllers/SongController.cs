using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.SongService;

namespace MusicStreamingService_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SongResponseModel>> GetSong(int id)
        {
            try
            {
                var song = await _songService.FindById(id);
                return Ok(song);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<SongResponseModel>>> GetAllSongs()
        {
            var songs = await _songService.GetAll();
            return Ok(songs);
        }

        [HttpPost]
        public async Task<ActionResult<SongResponseModel>> CreateSong([FromBody] SongRequestModel request)
        {
            try
            {
                var song = await _songService.CreateSong(request);
                return CreatedAtAction(nameof(GetSong), new { id = song.SongId }, song);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SongResponseModel>> DeleteSong(int id)
        {
            try
            {
                var song = await _songService.DeleteById(id);
                return Ok(song);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}