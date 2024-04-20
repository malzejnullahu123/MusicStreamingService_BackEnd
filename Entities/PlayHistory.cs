using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class PlayHistory
{
    public int PlayHistoryId { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; } // Foreign Key referencing User
    [ForeignKey("SongId")]
    public int SongId { get; set; } // Foreign Key referencing Song
    public DateTime DatePlayed { get; set; }

    // Navigation Properties for Relationships (Many-to-One)
    public User User { get; set; } // One play history entry belongs to one user (Many-to-One)
    public Song Song { get; set; } // One play history entry is for one song (Many-to-One)
}
