using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities
{
    public class Song
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SongId { get; set; }

        public string Title { get; set; }

        [ForeignKey("AlbumId")]
        public int? AlbumId { get; set; } // Foreign Key referencing Album

        [ForeignKey("ArtistId")]
        public int ArtistId { get; set; } // Foreign Key referencing Artist

        [ForeignKey("GenreId")]
        public int GenreId { get; set; } // Foreign Key referencing Genre

        public string EmbedLink { get; set; } // Path to audio file
        public string EmbedIMGLink { get; set; } // Path to audio file


        // Navigation Properties for Relationships (Many-to-One and Many-to-Many)
        public Album Album { get; set; } // One song belongs to one album (Many-to-One)

        public Artist Artist { get; set; } // One song belongs to one artist (Many-to-One)

        public Genre Genre { get; set; } // One song belongs to one genre (Many-to-One)

        public ICollection<PlaylistSong> Playlists { get; set; } // Song can be in many playlists (Many-to-Many)
    }
}