using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.PlaylistService;

namespace MusicStreamingService_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<PlaylistResponseModel>>> GetAllPlaylists()
        {
            try
            {
                var playlists = await _playlistService.GetAll();
                return Ok(playlists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistResponseModel>> GetPlaylistById(int id)
        {
            try
            {
                var playlist = await _playlistService.FindById(id);
                return Ok(playlist);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PlaylistResponseModel>> CreatePlaylist([FromBody] PlaylistRequestModel request)
        {
            try
            {
                var playlist = await _playlistService.CreatePlaylist(request);
                return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.PlaylistId }, playlist);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PlaylistResponseModel>> DeletePlaylist(int id)
        {
            try
            {
                var playlist = await _playlistService.DeleteById(id);
                return Ok(playlist);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        
        
        //crd for songs on playlist
        [HttpGet("{playlistId}/all-songs")]
        public async Task<ActionResult<List<SongResponseModel>>> GetSongsInPlaylist(int playlistId)
        {
            try
            {
                var songs = await _playlistService.GetSongsInPlaylist(playlistId);
                return Ok(songs);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{playlistId}/add-song/{songId}")]
        public async Task<ActionResult> AddSongToPlaylist(int playlistId, int songId)
        {
            try
            {
                await _playlistService.AddSongToPlaylist(playlistId, songId);
                return Ok("Song added to playlist successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding the song to the playlist.");
            }
        }

        [HttpDelete("{playlistId}/remove-song/{songId}")]
        public async Task<ActionResult> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            try
            {
                await _playlistService.RemoveSongFromPlaylist(playlistId, songId);
                return Ok("Song removed successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
