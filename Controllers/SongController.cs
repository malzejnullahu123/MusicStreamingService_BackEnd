using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.SongService;

namespace MusicStreamingService_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly ISongService _songService;

        public SongController(ILogger<SongController> logger, ISongService songService)
        {
            _logger = logger;
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

        // [HttpGet("all")]
        // public async Task<ActionResult<List<SongResponseModel>>> GetAllSongs()
        // {
        //     var songs = await _songService.GetAll();
        //     return Ok(songs);
        // }
        
        [HttpGet("new")]
        public async Task<ActionResult<List<SongResponseModel>>> GetNewSongs([FromQuery] int pageSize)
        {
            try
            {
                var songs = await _songService.GetNew(pageSize);
                return Ok(songs);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }
        
        [HttpGet("all/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<List<SongResponseModel>>> GetAllSongsByTen(int pageNumber, int pageSize)
        {
            try
            {
                var songs = await _songService.GetAll(pageNumber, pageSize);
                return Ok(songs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting songs.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the songs.");
            }
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
        
        [HttpGet("byGenre/{genreId}")]
        public async Task<ActionResult<List<SongResponseModel>>> GetSongsByGenre(int genreId)
        {
            var songs = await _songService.GetSongsByGenre(genreId);
            return Ok(songs);
        }
        
        [HttpGet("byArtist/{artistId}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<List<SongResponseModel>>> GetSongsByArtist(int artistId, int pageNumber, int pageSize)
        {
            try
            {
                var songs = await _songService.GetSongsByArtist(artistId, pageNumber, pageSize);
                return Ok(songs);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }
        
        [HttpGet("byAlbum/{albumId}")]
        public async Task<ActionResult<List<SongResponseModel>>> GetSongsByAlbum(int albumId)
        {
            try
            {
                var songs = await _songService.GetSongsByAlbum(albumId);
                return Ok(songs);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("recommended/a")]
        public async Task<ActionResult<List<SongResponseModel>>> GetRecommendedSongs()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var songs = await _songService.GetRecommendedSongs(token);
            return Ok(songs);
        }

    }
}