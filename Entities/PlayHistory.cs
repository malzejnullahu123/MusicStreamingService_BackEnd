using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class PlayHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PlayHistoryId { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; } // Foreign Key referencing User
    [ForeignKey("SongId")]
    public int SongId { get; set; } // Foreign Key referencing Song
    public DateTime DatePlayed { get; set; }

    public User User { get; set; } 
    public Song Song { get; set; } 
}
