using Microsoft.AspNetCore.Mvc;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services;
using MusicStreamingService_BackEnd.Services.AuthService;
using MusicStreamingService_BackEnd.Services.FollowService;

namespace MusicStreamingService_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IFollowService _followService;


    public UserController(ILogger<UserController> logger, IUserService userService, IAuthService authService, IFollowService followService)
    {
        _logger = logger;
        _userService = userService;
        _authService = authService;
        _followService = followService;
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
    
    // [HttpPost("login")]
    // public async Task<ActionResult<UserResponseModel>> Login(LoginRequestModel request)
    // {
    //     try
    //     {
    //         UserResponseModel user = await _authService.Authenticate(request);
    //         return Ok(user);
    //     }
    //     catch (ArgumentException e)
    //     {
    //         return NotFound(e.Message);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error authenticating user.");
    //         return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while authenticating the user.");
    //     }
    // }
    
    
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginRequestModel request)
    {
        try
        {
            string token = await _authService.Authenticate(request);
            return Ok(new { Token = token });
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while authenticating the user.");
        }
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserResponseModel>> Me([FromQuery] string token)
    {
        try
        {
            var user = await _authService.Me(token);
            return Ok(user);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while authenticating the user.");
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
    public async Task<ActionResult<List<UserResponseModel>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var users = await _userService.GetAllUsers(pageNumber, pageSize);
        return Ok(users);
    }


    [HttpDelete("myAcc/{id}")]
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
            
            return Ok(user);

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
    
        [HttpPost("follow/{followingUserId}")]
        // [Authorize]
        public async Task<IActionResult> FollowUser(int followingUserId)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                var result = await _followService.FollowUser(token, followingUserId);
                if (result)
                {
                    return Ok("User followed successfully.");
                }
                return BadRequest("User is already followed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error following user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while following the user.");
            }
        }

        [HttpPost("unfollow/{followingUserId}")]
        // [Authorize]
        public async Task<IActionResult> UnfollowUser(int followingUserId)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                var result = await _followService.UnfollowUser(token, followingUserId);
                if (result)
                {
                    return Ok("User unfollowed successfully.");
                }
                return BadRequest("User is not followed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unfollowing user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while unfollowing the user.");
            }
        }

        [HttpGet("isfollowing/{followingUserId}")]
        public async Task<IActionResult> IsFollowing(int followingUserId)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                var result = await _followService.IsFollowing(token, followingUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking follow status.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while checking follow status.");
            }
        }

        [HttpGet("allFollows/{id}")]
        public async Task<IActionResult> GetFollowCounts(int id)
        {
            var followerCounts = await _followService.GetFollowerCounts(id);
            return Ok(followerCounts);
        }
    
}