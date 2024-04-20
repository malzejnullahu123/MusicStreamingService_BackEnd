using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services;

namespace MusicStreamingService_BackEnd.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService iUserService)
    {
        _logger = logger;
        _userService = iUserService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseModel>> Create([FromBody] UserRequestModel request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Invalid request. Please provide all required fields.");
        }

        try
        {
            var user = await _userService.CreateUser(request);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Edit(int id, [FromBody] EditUserRequestModel request)
    {
        try
        {
            var user = await _userService.EditUser(id, request);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while editing the user.");
        }
    }


    
    [HttpGet("all")]
    public async Task<ActionResult<List<UserResponseModel>>> GetAll()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserResponseModel>> DeleteById(int id)
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
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseModel>> GetById(int id)
    {
        if (id == null)
        {
            return BadRequest("Please provide ID");
        }
        
        try
        {
            var user = await _userService.FindById(id);
        
            var userDto = new UserResponseModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email
            };
            return Ok(userDto);

        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while looking for the user.");
        }
        
    }
    
}