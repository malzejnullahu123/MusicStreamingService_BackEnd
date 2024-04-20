using System;
using System.Text.Json.Serialization;
using MusicStreamingService_BackEnd.Entities;

namespace MusicStreamingService_BackEnd.Models
{
    public class AlbumResponseModel
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public string Image { get; set; }

        [JsonIgnore]
        public Artist Artist { get; set; }
        [JsonIgnore]
        public List<Song> Songs { get; set; }
    }
}