using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Services.ArtistService;

namespace MusicStreamingService_BackEnd.Controllers;

public class ArtistController : ControllerBase
{
    private readonly ILogger<ArtistController> _logger;
    private readonly IArtistService _artistService;

    public ArtistController(ILogger<ArtistController> logger, IArtistService iArtistService)
    {
        _logger = logger;
        _artistService = iArtistService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<ArtistResponseModel>> Create([FromBody] ArtistRequestModel request)
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
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating artist.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the artist.");
        }
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<ArtistResponseModel>>> GetAll()
    {
        var artists = await _artistService.GetAllArtists();
        return Ok(artists);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ArtistResponseModel>> DeleteById(int id)
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
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistResponseModel>> GetById(int id)
    {
        if (id == null)
        {
            return BadRequest("Please provide ID");
        }

        try
        {
            var artist = await _artistService.FindById(id);
        
            return Ok(artist);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error lokking for the artist.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while looking for the artist.");
        }
        
    }
    
}