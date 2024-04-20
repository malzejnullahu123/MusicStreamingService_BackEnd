using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.AlbumService;

namespace MusicStreamingService_BackEnd.Controllers;

[ApiController]
[Route("[controller]")]
public class AlbumController : ControllerBase
{
    private readonly ILogger<AlbumController> _logger;
    private readonly IAlbumService _albumService;
    
    public AlbumController(ILogger<AlbumController> logger, IAlbumService iAlbumService)
    {
        _logger = logger;
        _albumService = iAlbumService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<Album>> CreateAlbum([FromBody] AlbumRequestModel album)
    {
        if (album == null)
        {
            return BadRequest();
        }

        var createdAlbum = await _albumService.CreateAlbum(album);

        return Ok(createdAlbum);
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<Album>>> GetAll()
    {
        var albums = await _albumService.GetAll();
        return Ok(albums);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Artist>> GetById(int id)
    {
        if (id == null)
        {
            return BadRequest("Please provide ID");
        }

        try
        {
            var album = await _albumService.FindById(id);
        
            return Ok(album);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error looking for album.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while looking for the album.");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Album>> DeleteById(int id)
    {
        try
        {
            var album = await _albumService.DeleteById(id);
            return Ok(album);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting album.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the album.");
        }
    }
    
}