using System.ComponentModel.DataAnnotations;

namespace MusicStreamingService_BackEnd.Models;

public class UserResponseModel
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string EmbedImgLink { get; set; }
    [EmailAddress]
    public string Email { get; set; }
}