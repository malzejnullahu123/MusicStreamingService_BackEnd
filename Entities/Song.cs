using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingService_BackEnd.Entities;

public class Song
{
    public int SongId { get; set; }
    public string Title { get; set; }
    [ForeignKey("AlbumId")]
    public int? AlbumId { get; set; } // Foreign Key referencing Album
    [ForeignKey("ArtistId")]
    public int ArtistId { get; set; } // Foreign Key referencing Artist
    public string EmbedLink { get; set; } // Path to audio file

    // Navigation Properties for Relationships (Many-to-One and Many-to-Many)
    public Album Album { get; set; } // One song belongs to one album (Many-to-One)
    public Artist Artist { get; set; } // One song belongs to one artist (Many-to-One)
    public ICollection<PlaylistSong> Playlists { get; set; } // Song can be in many playlists (Many-to-Many)
}