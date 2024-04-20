using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.ArtistService;

public class ArtistService : IArtistService
{
    private readonly AppDbContext _dbContext;

    public ArtistService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }
    
    public async Task<ArtistResponseModel> CreateArtist([FromBody] ArtistRequestModel request)
    {
        var artist = new Artist
        {
            // ArtistId = _dbContext.Artists.Count() + 1,  //Guid.NewGuid().ToString(),
            Name = request.Name
        };

        _dbContext.Artists.Add(artist);
        await _dbContext.SaveChangesAsync();

        return new ArtistResponseModel
        {
            ArtistId = artist.ArtistId,
            Name = request.Name
        };
    }

    public async Task<List<ArtistResponseModel>> GetAllArtists()
    {
        var artists = await _dbContext.Artists.ToListAsync();
        return artists.Select(artist => new ArtistResponseModel
        {
            ArtistId = artist.ArtistId,
            Name = artist.Name,
            // Map other properties as needed
        }).ToList();
    }

    public async Task<ArtistResponseModel> FindById(int id)
    {
        var artist = await _dbContext.Artists.FindAsync(id);
        if (artist == null)
        {
            throw new ArgumentException($"Artist with ID {id} not found.");
        }
        
        return new ArtistResponseModel
        {
            ArtistId = id,
            Name = artist.Name
        };
    }

    public async Task<ArtistResponseModel> DeleteById(int id)
    {
        var artist = await _dbContext.Artists.FindAsync(id);
        if (artist == null)
        {
            throw new ArgumentException($"Artist with ID {id} not found.");
        }

        _dbContext.Artists.Remove(artist);
        await _dbContext.SaveChangesAsync();

        return new ArtistResponseModel
        {
            ArtistId = artist.ArtistId,
            Name = artist.Name
        };
    }
}