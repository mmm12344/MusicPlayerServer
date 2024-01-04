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

        public string DbPath { get; }
        public MusicPlayerServerContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Sounders.db");
        }

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
            optionsBuilder.UseSqlite($"Data Source={DbPath}"); 
        }
    }
}
