using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayerServer.Models
{
    //playlist table 
    //contains columns:
    //1-playListID primary key
    //2-name
    //3-userId foreignkey to the user table (many to one relationship) 
    public class Playlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlaylistID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public byte[]? Picture { get; set; }

        
        public int UserID { get; set; }
        public User User { get; set; }

        public ICollection<SongPlaylist> SongPlaylists { get; set; }
    }
}
