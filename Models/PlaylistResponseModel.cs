using MusicStreamingService_BackEnd.Entities;

namespace MusicStreamingService_BackEnd.Models;

public class PlaylistResponseModel
{
    public int PlaylistId { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public string Image { get; set; } // URL to album image

    public bool IsVisible { get; set; }
    // public List<Song> Songs { get; set; }
}