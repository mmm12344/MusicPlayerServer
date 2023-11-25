using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayerServer.Models
{
    // userLikes Table used to achieve many to many realation ship between user and song table 
    // table description: used to save likes submitted from user to a song
    // contains columns:
    // 1- userLikedID (primaryKey)
    // 2- userID (foreign key to user table many to one relationship)
    // 3- songID (foreign key to song table many to one realtionship)
    public class UserLikes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserLikesID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Song")]
        public int SongID { get; set; }
        public Song Song { get; set; }
    }
}
