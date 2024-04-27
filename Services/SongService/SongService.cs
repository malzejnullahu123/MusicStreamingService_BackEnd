using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;

namespace MusicStreamingService_BackEnd.Services.SongService;

public class SongService : ISongService
{
    private readonly AppDbContext _dbContext;
    private readonly ExtractFromToken _extractor;

    public SongService(AppDbContext appDbContext, ExtractFromToken extractor)
    {
        _dbContext = appDbContext;
        _extractor = extractor;
    }

    public async Task<SongResponseModel> FindById(int id)
    {
        var song = await _dbContext.Songs
            .Include(s => s.Artist)
            .Include(s => s.Genre)
            .FirstOrDefaultAsync(s => s.SongId == id);

        if (song == null)
        {
            throw new ArgumentException($"Song with ID {id} not found.");
        }

        return new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            ArtistName = song.Artist.Name,
            GenreId = song.GenreId,
            GenreName = song.Genre.Name,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        };
    }

    public async Task<List<SongResponseModel>> GetNew(int pageSize)
    {
        var songs = await _dbContext.Songs
            .Include(song => song.Artist)
            .Include(song => song.Genre)
            .OrderByDescending(song => song.SongId)
            .Take(pageSize)
            .ToListAsync();

        return songs.Select(song => new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            ArtistName = song.Artist.Name,
            GenreId = song.GenreId,
            GenreName = song.Genre.Name,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        }).ToList();
    }


    // public async Task<List<SongResponseModel>> GetAll()
    // {
    //     var songs = await _dbContext.Songs
    //         .Include(song => song.Artist)
    //         .Include(song => song.Genre)
    //         .OrderByDescending(song => song.SongId)
    //         .ToListAsync();
    //
    //     return songs.Select(song => new SongResponseModel
    //     {
    //         SongId = song.SongId,
    //         Title = song.Title,
    //         ArtistId = song.ArtistId,
    //         ArtistName = song.Artist.Name,
    //         GenreId = song.GenreId,
    //         GenreName = song.Genre.Name,
    //         EmbedLink = song.EmbedLink,
    //         EmbedIMGLink = song.EmbedIMGLink
    //     }).ToList();
    // }
    
    public async Task<List<SongResponseModel>> GetAll(int pageNumber, int pageSize)
    {
        var songs = await _dbContext.Songs
            .Include(song => song.Artist)
            .Include(song => song.Genre)
            .OrderBy(song => song.SongId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return songs.Select(song => new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            ArtistName = song.Artist.Name,
            GenreId = song.GenreId,
            GenreName = song.Genre.Name,
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
    
    public async Task<List<SongResponseModel>> GetSongsByGenre(int genreId)
    {
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.GenreId == genreId);
        if (genre == null)
        {
            throw new InvalidOperationException("Genre with the specified ID does not exist in the database.");
        }

        var songs = await _dbContext.Songs
            .Where(song => song.GenreId == genreId)
            .ToListAsync();

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
    
    public async Task<List<SongResponseModel>> GetSongsByArtist(int artistId, int pageNumber, int pageSize)
    {
        // Calculate the number of records to skip based on the page number and page size
        int skip = (pageNumber - 1) * pageSize;

        var songs = await _dbContext.Songs
            .Where(song => song.ArtistId == artistId)
            .OrderByDescending(song => song.SongId) // It's a good practice to order the results for consistent pagination
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        if (songs.Count == 0)
        {
            throw new ArgumentException($"No songs found for artist with ID {artistId}");
        }

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

    
    public async Task<List<SongResponseModel>> GetSongsByAlbum(int albumId)
    {
        var songs = await _dbContext.Songs
            .Where(s => s.AlbumId == albumId)
            .Include(song => song.Artist)
            .Include(song => song.Genre)
            .ToListAsync();

        if (songs.Count == 0)
        {
            throw new ArgumentException($"No songs found for album with ID {albumId}");
        }

        return songs.Select(song => new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            ArtistName = song.Artist.Name,
            GenreId = song.GenreId,
            GenreName = song.Genre.Name,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        }).ToList();
    }
    
    public async Task<List<SongResponseModel>> GetRecommendedSongs(string token)
    {
        var userId = _extractor.Id(token);
        // Get the genre ids of songs the user listened to
        var userGenreIds = await _dbContext.PlayHistories
            .Where(ph => ph.UserId == userId)
            .Select(ph => ph.Song.GenreId)
            .Distinct()
            .ToListAsync();

        // Get the artist ids of songs the user listened to
        var userArtistIds = await _dbContext.PlayHistories
            .Where(ph => ph.UserId == userId)
            .Select(ph => ph.Song.ArtistId)
            .Distinct()
            .ToListAsync();

        // Get the ids of trendy songs
        var trendySongIds = await _dbContext.PlayHistories
            .GroupBy(ph => ph.SongId)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Take(10) // Assuming you want the top 10 trendy songs
            .ToListAsync();

        var recommendedSongs = await _dbContext.Songs
            .Include(song => song.Artist)
            .Include(song => song.Genre)
            .Where(song =>
                (userGenreIds.Contains(song.GenreId) || userArtistIds.Contains(song.ArtistId) || trendySongIds.Contains(song.SongId))
                && song.SongId != userId) // Exclude songs the user has listened to
            .ToListAsync();

        return recommendedSongs.Select(song => new SongResponseModel
        {
            SongId = song.SongId,
            Title = song.Title,
            ArtistId = song.ArtistId,
            ArtistName = song.Artist.Name,
            GenreId = song.GenreId,
            GenreName = song.Genre.Name,
            EmbedLink = song.EmbedLink,
            EmbedIMGLink = song.EmbedIMGLink
        }).ToList();
    }

   


}
