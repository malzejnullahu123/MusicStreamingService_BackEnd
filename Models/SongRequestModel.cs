namespace MusicStreamingService_BackEnd.Models;

public class SongRequestModel
{
    public string Title { get; set; }
    public int ArtistId { get; set; }
    public int GenreId { get; set; }
    public string EmbedLink { get; set; }
    public string EmbedIMGLink { get; set; }
}