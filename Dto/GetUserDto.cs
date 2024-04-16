using System.ComponentModel.DataAnnotations;

namespace MusicStreamingService_BackEnd.Dto;

public class GetUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    [EmailAddress]
    public string Email { get; set; }
}