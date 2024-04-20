namespace MusicStreamingService_BackEnd.Entities;

public class User
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    

    // Navigation Properties for Relationships (One-to-Many and Many-to-Many)
    public ICollection<Playlist> Playlists { get; set; } // One user can have many playlists
    public ICollection<PlayHistory> PlayHistories { get; set; } // Optional: One user can have many play history entries
}