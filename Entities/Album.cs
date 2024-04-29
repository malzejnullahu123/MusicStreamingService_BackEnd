using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class Album
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AlbumId { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    [ForeignKey("ArtistId")]
    public int ArtistId { get; set; } // Foreign Key referencing Artist
    public string Image { get; set; }

    public Artist Artist { get; set; } // One album belongs to one artist (Many-to-One)
    public ICollection<Song> Songs { get; set; } // One album can have many songs (One-to-Many)
}