namespace MusicStreamingService_BackEnd.Models
{
    public class PlayHistoryResponseModel
    {
        public int PlayHistoryId { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }
        public DateTime DatePlayed { get; set; }
        public SongResponseModel Song { get; set; }
    }
}