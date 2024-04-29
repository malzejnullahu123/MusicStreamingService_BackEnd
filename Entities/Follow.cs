using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MusicStreamingService_BackEnd.Entities
{
    public class Follow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FollowId { get; set; }

        [ForeignKey("UserId")]
        public int UserID { get; set; } // Foreign Key referencing User (following user)

        [ForeignKey("UserId")]
        public int FollowingUserID { get; set; } // Foreign Key referencing User (user being followed)

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public User FollowingUser { get; set; } 
    }
}