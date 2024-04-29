using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicStreamingService_BackEnd.Entities;

namespace MusicStreamingService_BackEnd.Models;

public class AlbumRequestModel
{
    public string Title { get; set; }
    public string Image { get; set; }

    // [JsonIgnore] public Artist Artist { get; set; }
    // [JsonIgnore]
    // public List<Song> Songs { get; set; } = new List<Song>();

}