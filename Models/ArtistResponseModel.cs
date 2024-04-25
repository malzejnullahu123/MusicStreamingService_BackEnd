namespace MusicStreamingService_BackEnd.Models;

public class ArtistResponseModel
{
    public int ArtistId { get; set; }
    public string Name { get; set; }
    public string EmbedImgLink { get; set; }
    public List<string> Albums { get; set; }
}