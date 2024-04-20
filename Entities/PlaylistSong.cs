using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicStreamingService_BackEnd.Entities;

[PrimaryKey(("PlaylistId"),("SongId"))]
public class PlaylistSong
{
    [ForeignKey("PlaylistId")]
    public int PlaylistId { get; set; } // Foreign Key referencing Playlist
    [ForeignKey("SongId")]
    public int SongId { get; set; } // Foreign Key referencing Song

    // Composite Primary Key
    public Playlist Playlist { get; set; }
    public Song Song { get; set; }
}