using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.SongService;

public class SongService : ISongService
{
    private readonly AppDbContext _dbContext;

    public SongService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public async Task<SongResponseModel> FindById(int id)
    {
        var song = await _dbContext.Songs.FindAsync(id);
        if (song == null)
        {
            throw new ArgumentException($"Song with ID {id} not found.");
        }

        return new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            GenreId = song.GenreId,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        };
    }

    public async Task<List<SongResponseModel>> GetAll()
    {
        var songs = await _dbContext.Songs.ToListAsync();
        return songs.Select(song => new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            GenreId = song.GenreId,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        }).ToList();
    }

    public async Task<SongResponseModel> CreateSong(SongRequestModel request)
    {
        var artist = await _dbContext.Artists.FirstOrDefaultAsync(a => a.ArtistId == request.ArtistId);
        if (artist == null)
        {
            throw new InvalidOperationException("Artist with the specified ID does not exist in the database.");
        }

        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.GenreId == request.GenreId);
        if (genre == null)
        {
            throw new InvalidOperationException("Genre with the specified ID does not exist in the database.");
        }

        var song = new Song
        {
            Title = request.Title,
            ArtistId = request.ArtistId,
            GenreId = request.GenreId,
            EmbedLink = request.EmbedLink,
            EmbedIMGLink = request.EmbedIMGLink
        };

        _dbContext.Songs.Add(song);
        await _dbContext.SaveChangesAsync();

        return new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            GenreId = song.GenreId,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        };
    }

    public async Task<SongResponseModel> DeleteById(int id)
    {
        var song = await _dbContext.Songs.FindAsync(id);
        if (song == null)
        {
            throw new ArgumentException($"Song with ID {id} not found.");
        }

        _dbContext.Songs.Remove(song);
        await _dbContext.SaveChangesAsync();

        return new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            GenreId = song.GenreId,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        };
    }
}