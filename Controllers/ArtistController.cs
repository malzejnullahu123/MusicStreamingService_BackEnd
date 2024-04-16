using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Dto;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.ArtistService;

namespace MusicStreamingService_BackEnd.Controllers;

public class ArtistController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ArtistService _artistService;

    public ArtistController(ILogger<UserController> logger, ArtistService artistService)
    {
        _logger = logger;
        _artistService = artistService;
    }
    
    [HttpPost("CreateArtist")]
    public async Task<ActionResult<Artist>> Create([FromBody] ArtistDto request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Invalid request. Please provide Artist Name");
        }

        try
        {
            var artist = await _artistService.CreateArtist(request);
            return Ok(artist);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
        }
    }
    
    [HttpGet("All")]
    public async Task<ActionResult<List<Artist>>> GetAll()
    {
        var artists = await _artistService.GetAllArtists();
        return Ok(artists);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Artist>> DeleteById(int id)
    {
        try
        {
            var artist = await _artistService.DeleteById(id);
            return Ok(artist);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting artist.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the artist.");
        }
    }
    
    [HttpGet("ById")]
    public async Task<ActionResult<Artist>> GetById(int id)
    {
        if (id == null)
        {
            return BadRequest("Please provide ID");
        }
        
        var artist = await _artistService.FindById(id);
        
        return Ok(artist);
    }
    
}