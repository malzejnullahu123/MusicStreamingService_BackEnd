namespace MusicStreamingService_BackEnd.Models;

public class PlaylistRequestModel
{
    public string Name { get; set; }
    public int UserId { get; set; }
    public string Image { get; set; } // URL to album image

    public bool IsVisible { get; set; }
    
    // public List<int> SongIds { get; set; }
}