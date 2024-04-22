using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.GenreService;

namespace MusicStreamingService_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreResponseModel>> GetGenreById(int id)
        {
            try
            {
                var genre = await _genreService.FindById(id);
                return Ok(genre);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<GenreResponseModel>>> GetAllGenres()
        {
            var genres = await _genreService.GetAll();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<ActionResult<GenreResponseModel>> CreateGenre([FromBody] GenreRequestModel request)
        {
            try
            {
                var genre = await _genreService.CreateGenre(request);
                return CreatedAtAction(nameof(GetGenreById), new { id = genre.GenreId }, genre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenreResponseModel>> DeleteGenreById(int id)
        {
            try
            {
                var genre = await _genreService.DeleteById(id);
                return Ok(genre);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
