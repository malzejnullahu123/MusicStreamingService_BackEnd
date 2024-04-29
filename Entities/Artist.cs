using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class Artist
{
    [Key]
    public int ArtistId { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; } // Foreign key property
    public string Name { get; set; }
    public string EmbedImgLink { get; set; }

    
    public ICollection<Album> Albums { get; set; } // One artist can have many albums
    public User User { get; set; }
}