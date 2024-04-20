using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class Playlist
{
    public int PlaylistId { get; set; }
    public string Name { get; set; }
    [ForeignKey("Userid")]
    public int UserId { get; set; } // Foreign Key referencing User

    // Navigation Properties for Relationships (Many-to-One and Many-to-Many)
    public User User { get; set; } // One playlist belongs to one user (Many-to-One)
    public ICollection<PlaylistSong> Songs { get; set; } // Playlist can contain many songs (Many-to-Many)
}