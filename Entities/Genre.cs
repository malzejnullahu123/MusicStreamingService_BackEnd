using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GenreId { get; set; }
    public string Name { get; set; }

    // Navigation Property for Relationship (Many-to-Many)
    public ICollection<Song> Songs { get; set; } // Genre can be associated with many songs
}