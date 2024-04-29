using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.AlbumService;

namespace MusicStreamingService_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        string token = HttpContext.Request.Headers["Authorization"];

        if (album == null)
        {
            return BadRequest();
        }

        var createdAlbum = await _albumService.CreateAlbum(token, album);

        return Ok(createdAlbum);
    }
    
    [HttpGet("all/{pageNumber}/{pageSize}")]
    public async Task<ActionResult<List<Album>>> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var albums = await _albumService.GetAll(pageNumber, pageSize);
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
    
    [HttpPost("{albumId}/songs/{songId}")]
    public async Task<ActionResult<AlbumResponseModel>> AddSongToAlbum(int albumId, int songId)
    {
        try
        {
            var album = await _albumService.AddSongToAlbum(albumId, songId);
            return Ok(album);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding song to album.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the song to the album.");
        }
    }
    
}