using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayerServer.Models
{
    // song table
    // contains columns:
    // 1- songId (primaryKey)
    // 2- name
    // 3- file 
    // 4- likes
    // 5- userId (foreign key to user table many to one relationship)
    public class Song
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SongID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public byte[] File { get; set; }

        public int? Likes { get; set; }

        public byte[]? Picture { get; set; }
        
        public int UserID { get; set; }
        public User User { get; set; }

        public ICollection<SongPlaylist> SongPlaylists { get; set; }
        public ICollection<UserLikes> LikedByUsers { get; set; }
    }
}
