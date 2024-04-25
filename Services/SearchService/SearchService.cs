using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace MusicStreamingService_BackEnd.Services.SearchService;

public class SearchService : ISearchService
{
    private readonly AppDbContext _dbContext;

    public SearchService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SongResponseModel>> SearchSongs(string query, int pageNumber, int pageSize)
    {
        var songs = await _dbContext.Songs
            .Where(s => EF.Functions.Like(s.Title.ToUpper(), $"%{query.ToUpper()}%")
                        || EF.Functions.Like(s.Artist.Name.ToUpper(), $"%{query.ToUpper()}%"))
            .Include(song => song.Artist)
            .Include(song => song.Genre)
            .OrderBy(song => EF.Functions.Like(song.Title.ToUpper(), $"%{query.ToUpper()}%") ? 0 : 1) // Order by similarity in title
            .ThenBy(song => EF.Functions.Like(song.Artist.Name.ToUpper(), $"%{query.ToUpper()}%") ? 0 : 1) // Then order by similarity in artist name
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (songs.Count == 0)
        {
            throw new ArgumentException($"No songs found for query: {query}");
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

    public async Task<List<ArtistResponseModel>> SearchArtists(string query, int pageNumber, int pageSize)
    {
        var artists = await _dbContext.Artists
            .Where(a => EF.Functions.Like(a.Name.ToUpper(), $"%{query.ToUpper()}%"))
            .OrderBy(a => EF.Functions.Like(a.Name.ToUpper(), $"%{query.ToUpper()}%") ? 0 : 1) // Order by similarity in artist name
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (artists.Count == 0)
        {
            throw new ArgumentException($"No artists found for query: {query}");
        }

        return artists.Select(artist => new ArtistResponseModel
        {
            ArtistId = artist.ArtistId,
            Name = artist.Name,
            EmbedImgLink = artist.EmbedImgLink
        }).ToList();
    }

    public async Task<List<AlbumResponseModel>> SearchAlbums(string query, int pageNumber, int pageSize)
    {
        var albums = await _dbContext.Albums
            .Where(a => EF.Functions.Like(a.Title.ToUpper(), $"%{query.ToUpper()}%")
                        || EF.Functions.Like(a.Artist.Name.ToUpper(), $"%{query.ToUpper()}%"))
            .OrderBy(a => EF.Functions.Like(a.Title.ToUpper(), $"%{query.ToUpper()}%") ? 0 : 1) // Order by similarity in album title
            .Include(album => album.Artist)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (albums.Count == 0)
        {
            throw new ArgumentException($"No albums found for query: {query}");
        }

        return albums.Select(album => new AlbumResponseModel
        {
            AlbumId = album.AlbumId,
            Title = album.Title,
            ReleaseDate = album.ReleaseDate,
            ArtistId = album.ArtistId,
            ArtistName = album.Artist.Name,
            Image = album.Image
        }).ToList();
    }

}