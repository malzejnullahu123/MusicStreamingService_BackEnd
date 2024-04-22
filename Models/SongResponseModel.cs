namespace MusicStreamingService_BackEnd.Models;

public class SongResponseModel
{
    public int SongId { get; set; }
    public string Title { get; set; }
    public int ArtistId { get; set; }
    public int GenreId { get; set; }
    public string EmbedLink { get; set; }
    public string EmbedIMGLink { get; set; }
}