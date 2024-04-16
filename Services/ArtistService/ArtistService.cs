using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Dto;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.ArtistService;

public class ArtistService : IArtistService
{
    private readonly AppDbContext _dbContext;

    public ArtistService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }
    
    public async Task<Artist> CreateArtist([FromBody] ArtistDto request)
    {
        var artist = new Artist
        {
            ArtistId = _dbContext.Artists.Count() + 1,
            Name = request.Name
        };

        _dbContext.Artists.Add(artist);
        await _dbContext.SaveChangesAsync();

        return artist;
    }

    public async Task<List<Artist>> GetAllArtists()
    {
        var artists = await _dbContext.Artists.ToListAsync();
        return artists;
    }

    public async Task<Artist> FindById(int id)
    {
        var artist = await _dbContext.Artists.FindAsync(id);
        if (artist == null)
        {
            throw new ArgumentException($"Artist with ID {id} not found.");
        }

        return artist;
    }

    public async Task<Artist> DeleteById(int id)
    {
        var artist = await _dbContext.Artists.FindAsync(id);
        if (artist == null)
        {
            throw new ArgumentException($"Artist with ID {id} not found.");
        }

        _dbContext.Artists.Remove(artist);
        await _dbContext.SaveChangesAsync();

        return artist;
    }
}