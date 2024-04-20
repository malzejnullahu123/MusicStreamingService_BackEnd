namespace MusicStreamingService_BackEnd.Entities;

public class Artist
{
    public int ArtistId { get; set; }
    public string Name { get; set; }
    
    // Navigation Property for Relationship (One-to-Many)
    public ICollection<Album> Albums { get; set; } // One artist can have many albums
}