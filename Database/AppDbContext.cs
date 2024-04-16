using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }
    
}