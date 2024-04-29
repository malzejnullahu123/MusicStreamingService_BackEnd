using Microsoft.EntityFrameworkCore;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Services;
using MusicStreamingService_BackEnd.Services.AlbumService;
using MusicStreamingService_BackEnd.Services.ArtistService;
using MusicStreamingService_BackEnd.Services.AuthService;
using MusicStreamingService_BackEnd.Services.FollowService;
using MusicStreamingService_BackEnd.Services.GenreService;
using MusicStreamingService_BackEnd.Services.PlayHistoryService;
using MusicStreamingService_BackEnd.Services.PlaylistService;
using MusicStreamingService_BackEnd.Services.SearchService;
using MusicStreamingService_BackEnd.Services.SongService;
using MusicStreamingService_BackEnd.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseNpgsql("Host=34.32.247.126;Port=5432;Database=music_db;Username=postgres1;Password=OHFl&&VHagi:6,%z"));
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IPlayHistoryService, PlayHistoryService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<ExtractFromToken>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://www.beatflow.live", "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();