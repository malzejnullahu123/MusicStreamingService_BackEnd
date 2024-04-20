namespace MusicStreamingService_BackEnd.Models;

public class UserRequestModel
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }
}