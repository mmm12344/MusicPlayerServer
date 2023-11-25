using Microsoft.EntityFrameworkCore;

namespace MusicPlayerServer.Models
{

    /*
     MusicPlayerServerContext is the class that will by asp.net to make queries and add or delete entries
        in the database
     */
    public class MusicPlayerServerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SongPlaylist> SongPlaylists { get; set; }
        public DbSet<UserLikes> UserLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Song>()
                .HasIndex(s => s.Name)
                .IsUnique();


            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MusicPlayerServerContext");
        }
    }
}
