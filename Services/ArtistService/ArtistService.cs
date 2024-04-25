using System.Security.Claims;
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
    
    public async Task<ArtistResponseModel> CreateArtist(string token, ArtistRequestModel request)
    {
        var principal = TokenService.VerifyToken(token);
    
        var idClaim = principal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        int.TryParse(idClaim.Value, out var id);
    
        // Check if the user already exists as an artist
        if (_dbContext.Artists.Any(a => a.UserId == id))
        {
            throw new ArgumentException("User is already an artist.");
        }
        
        var artist = new Artist
        {
            ArtistId = id,
            UserId = id,
            Name = request.Name,
            EmbedImgLink = request.EmbedImgLink
        };
        
        var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }
        
        user.EmbedImgLink = request.EmbedImgLink;

        _dbContext.Artists.Add(artist);
        await _dbContext.SaveChangesAsync();

        return new ArtistResponseModel
        {
            ArtistId = artist.ArtistId,
            Name = request.Name,
            EmbedImgLink = request.EmbedImgLink
        };
    }


    // public async Task<List<ArtistResponseModel>> GetAllArtists()
    // {
    //     var artists = await _dbContext.Artists.ToListAsync();
    //     return artists.Select(artist => new ArtistResponseModel
    //     {
    //         ArtistId = artist.ArtistId,
    //         Name = artist.Name,
    //         EmbedImgLink = artist.EmbedImgLink,
    //     }).ToList();
    // }
    
    public async Task<List<ArtistResponseModel>> GetAllArtists(int pageNumber, int pageSize)
    {
        var artists = await _dbContext.Artists
            .OrderByDescending(artist => artist.ArtistId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return artists.Select(artist => new ArtistResponseModel
        {
            ArtistId = artist.ArtistId,
            Name = artist.Name,
            EmbedImgLink = artist.EmbedImgLink,
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
            Name = artist.Name,
            EmbedImgLink = artist.EmbedImgLink
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
            Name = artist.Name,
            EmbedImgLink = artist.EmbedImgLink
        };
    }
}