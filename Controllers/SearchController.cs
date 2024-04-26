using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.SearchService;

namespace MusicStreamingService_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("songs")]
    public async Task<ActionResult<List<SongResponseModel>>> SearchSongs([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var songs = await _searchService.SearchSongs(query, pageNumber, pageSize);
            return Ok(songs);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("artists")]
    public async Task<ActionResult<List<ArtistResponseModel>>> SearchArtists([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var artists = await _searchService.SearchArtists(query, pageNumber, pageSize);
            return Ok(artists);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("albums")]
    public async Task<ActionResult<List<AlbumResponseModel>>> SearchAlbums([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var albums = await _searchService.SearchAlbums(query, pageNumber, pageSize);
            return Ok(albums);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet("users")]
    public async Task<ActionResult<List<UserResponseModel>>> SearchUsers([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var users = await _searchService.SearchUsers(query, pageNumber, pageSize);
            return Ok(users);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("playlists")]
    public async Task<ActionResult<List<PlaylistResponseModel>>> SearchPublicPlaylist([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var playlists = await _searchService.SearchPublicPlaylist(query, pageNumber, pageSize);
            return Ok(playlists);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}