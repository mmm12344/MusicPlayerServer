using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayerServer.Models
{
    // user table
    // contains columns:
    // 1- userID (primaryKey)
    // 2- firstName
    // 3- last Name
    // 4- email (uniquefield)
    // 5- password
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<UserLikes> LikedSongs { get; set; }
    }
}
