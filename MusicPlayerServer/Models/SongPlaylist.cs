using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicPlayerServer.Models
{
    // songplaylist table which is an intermediate table between song table and playlist table to achieve many to many relationship
    // contains columns:
    // 1- songplaylistID (primaryKey)
    // 2- songID (foreign key to song table many to one relationship)
    // 3- playlistID (foreign key to playlist table many to one relationship)
    public class SongPlaylist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SongPlaylistID { get; set; }

        [ForeignKey("Song")]
        public int SongID { get; set; }
        public Song Song { get; set; }

        [ForeignKey("Playlist")]
        public int PlaylistID { get; set; }
        public Playlist Playlist { get; set; }
    }
}
