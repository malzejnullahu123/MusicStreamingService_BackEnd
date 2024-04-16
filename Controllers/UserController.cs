using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MusicStreamingService_BackEnd.Dto;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.UserService;

namespace MusicStreamingService_BackEnd.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserService _userService;

    public UserController(ILogger<UserController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult<GetUserDto>> Create([FromBody] SignUpDto request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Invalid request. Please provide all required fields.");
        }

        try
        {
            var user = await _userService.CreateUser(request);
            var userDto = new GetUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email
            };
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
        }
    }



    
    [HttpGet("All")]
    public async Task<ActionResult<List<GetUserDto>>> GetAll()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<GetUserDto>> DeleteById(int id)
    {
        try
        {
            var artist = await _userService.DeleteById(id);
            return Ok(artist);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
        }
    }
    
    [HttpGet("ById")]
    public async Task<ActionResult<GetUserDto>> GetById(int id)
    {
        if (id == null)
        {
            return BadRequest("Please provide ID");
        }
        
        var user = await _userService.FindById(id);
        
        var userDto = new GetUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email
        };
        return Ok(userDto);
    }
    
}