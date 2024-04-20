using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.AlbumService;

public class AlbumService : IAlbumService
{
    private readonly AppDbContext _dbContext;

    public AlbumService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public async Task<Album> FindById(int id)
    {
        var album = await _dbContext.Albums.FindAsync(id);
        if (album == null)
        {
            throw new ArgumentException($"Album with ID {id} not found.");
        }

        return album;
    }

    public async Task<List<Album>> GetAll()
    {
        var albums = await _dbContext.Albums.ToListAsync();
        return albums;
    }

    public async Task<AlbumResponseModel> CreateAlbum(AlbumRequestModel request)
    {
        var artist = await _dbContext.Artists.FindAsync(request.ArtistId);
        if (artist == null)
        {
            throw new InvalidOperationException("Artist id doesn't exist in our database.");
        }

        var album = new Album
        {
            AlbumId = _dbContext.Albums.Count() + 1,
            Title = request.Title,
            ArtistId = request.ArtistId,
            ReleaseDate = DateTime.UtcNow,
            Image = request.Image
        };

        _dbContext.Albums.Add(album);
        await _dbContext.SaveChangesAsync();

        var albumResponse = new AlbumResponseModel
        {
            AlbumId = album.AlbumId,
            Title = album.Title,
            ReleaseDate = album.ReleaseDate,
            ArtistId = album.ArtistId,
            Image = album.Image,
            Artist = artist
        };

        return albumResponse;
    }


    public async Task<Album> DeleteById(int id)
    {
        var album = await _dbContext.Albums.FindAsync(id);
        if (album == null)
        {
            throw new ArgumentException($"Album with ID {id} not found.");
        }

        _dbContext.Albums.Remove(album);
        await _dbContext.SaveChangesAsync();

        return album;
    }

    public async Task<Album> CreateAlbum(Album album)
    {
        if (album == null)
        {
            throw new ArgumentNullException(nameof(album));
        }

        if (album.ReleaseDate == default)
        {
            album.ReleaseDate = DateTime.UtcNow;
        }

        _dbContext.Albums.Add(album);
        _dbContext.SaveChanges();

        return album;
    }
}