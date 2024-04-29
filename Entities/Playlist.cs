using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class Playlist
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PlaylistId { get; set; }
    public string Name { get; set; }
    [ForeignKey("Userid")]
    public int UserId { get; set; } // Foreign Key referencing User
    public string Image { get; set; } 

    public bool IsVisible { get; set; }

    public User User { get; set; } 
    public ICollection<PlaylistSong> Songs { get; set; } 
}