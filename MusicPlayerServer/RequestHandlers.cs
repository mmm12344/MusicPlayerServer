using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using MusicPlayerServer.Models;
using NuGet.Protocol;
using System.Net.Http;
using System.Security.Cryptography.Xml;

namespace MusicPlayerServer
{
    public record AddSongRecord(string name, byte[] file, byte[] picture);
    public record AddPlaylistRecord(string name, byte[] picture);

    public record SongInfo(int songID, string name, byte[] picture);
    public class RequestHandlers
    {
        private static MusicPlayerServerContext context = new MusicPlayerServerContext();
        public static async Task<IResult> GetLikedSongs(HttpContext httpContext)
        {
           
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));
            var query = from userLikes
                        in context.UserLikes
                        where userLikes.UserID == userID
                        select new 
                        { 
                            userLikes.SongID ,
                            userLikes.Song.Name,
                            userLikes.Song.Picture
                        };
            return Results.Ok(query);
        }

        public static async Task<IResult> GetOwnSongs(HttpContext httpContext)
        {

       
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));
            var query = from songs
                    in context.Songs
                    where songs.UserID == userID
                    select new
                    {
                        songs.SongID,
                        songs.Name,
                        songs.Picture
                    };
            return Results.Ok(query);
        }

        public static async Task<IResult> GetAllSongs()
        {
        
            var query = from songs
                    in context.Songs
                    select new
                    {
                        songs.SongID,
                        songs.Name,
                        songs.Picture
                    };
            return Results.Ok(query);
        }

        public static async Task<IResult> Search(string songName)
        {
            string[] songKeyWords = songName.Split(" ");

            var query = from songs
                    in context.Songs

                        select songs;
                    
            Dictionary<Song, int> songResultWeight = new();
            
            foreach(Song song in query)
            {
                int weight = 0;
                foreach (string word in songKeyWords)
                { 
                    if (song.Name.Contains(word))
                    {
                        weight++; 
                    }
                    
                }
                if(weight != 0)
                    songResultWeight.Add(song, weight);
            }

            songResultWeight.OrderBy(s => s.Value);

            List<SongInfo> songResults = new List<SongInfo>();

            for(int i = songResultWeight.Count()-1; i >= 0; i--)
            {
                var song = songResultWeight.ElementAt(i).Key;
                songResults.Add(new SongInfo(song.SongID, song.Name, song.Picture));
            }

            return Results.Ok(songResults);
        }

        public static async Task<IResult> GetAllPlaylists()
        {

            var query = from playlist
                    in context.Playlists
                    select new
                    {
                        playlist.PlaylistID,
                        playlist.Name,
                        playlist.Picture
                    };

            return Results.Ok(query);
        }

        public static async Task<IResult> GetOwnPlaylists(HttpContext httpContext)
        {
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));

            var query = from playlist
                    in context.Playlists
                    where playlist.UserID == userID
                    select new
                    {
                        playlist.PlaylistID,
                        playlist.Name,
                        playlist.Picture
                    };

            return Results.Ok(query);
        }

        public static async Task<IResult> GetPlayListSongs(int playlistID, HttpContext httpContext)
        {
            
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));

            var query = from songPlaylist
                        in context.SongPlaylists
                        where songPlaylist.PlaylistID == playlistID
                        select new
                        {
                            songPlaylist.SongID,
                            songPlaylist.Song.Name,
                            songPlaylist.Song.Picture
                        };

            return Results.Ok(query);
        }

        public static async Task<IResult> AddSong(AddSongRecord songToAdd, HttpContext httpContext)
        {
      
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));
            
            Song song = new Song { Name = songToAdd.name, File = songToAdd.file, Likes = 0, Picture = songToAdd.picture, UserID = userID };
            
            await context.Songs.AddAsync(song);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> AddPlaylist(AddPlaylistRecord playlistToAdd, HttpContext httpContext)
        {

            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));
            Playlist playlist = new Playlist { Name = playlistToAdd.name, Picture = playlistToAdd.picture, UserID = userID };

            await context.Playlists.AddAsync(playlist);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> AddSongToPlaylist(SongPlaylist songplaylist)
        {

            var query = from songPlaylist
                        in context.SongPlaylists
                        where songPlaylist.PlaylistID == songplaylist.SongID && songPlaylist.PlaylistID == songplaylist.PlaylistID
                        select songPlaylist;
            if (query.Count() > 0)
            {
                return Results.Problem();
            }


            await context.SongPlaylists.AddAsync(songplaylist);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> DeleteSong(int songID, HttpContext httpContext)
        {

            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));

            var query = from song
                        in context.Songs
                        where song.SongID == songID && song.UserID == userID
                        select song;
            context.Songs.Remove(query.FirstOrDefault());
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> AddLikeToSong(int songID, HttpContext httpContext)
        {

            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));

            var query = from userLike
                        in context.UserLikes
                        where userLike.UserID == userID && userLike.SongID == songID
                        select userLike;
            if(query.Count() > 0)
            {
                return Results.Ok();
            }

            UserLikes entry = new UserLikes { SongID = songID, UserID = userID };
            await context.UserLikes.AddAsync(entry);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> GetSong(int songID)
        {
            var query = from song
                    in context.Songs
                    where song.SongID == songID
                    select new
                    {
                        song.SongID,
                        song.Name,
                        song.File,
                        song.Likes,
                        song.Picture
                    };

            return Results.Ok(query.FirstOrDefault());
        }

        public static async Task<IResult> IsLiked(int songID, HttpContext httpContext)
        {
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));
            var query = from userLikes
                        in context.UserLikes
                        where userLikes.SongID == songID && userLikes.UserID == userID
                        select userLikes;

            if (query.Count() == 0)
                return Results.Ok(false);
            return Results.Ok(true);

        }

        public static async Task<IResult> RemoveLike(int songID, HttpContext httpContext)
        {
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));
            var query = from userLikes
                        in context.UserLikes
                        where userLikes.SongID == songID && userLikes.UserID == userID
                        select userLikes;
            if(query.Count() == 0)
            {
                return Results.Problem();
            }
            context.UserLikes.Remove(query.FirstOrDefault());
            context.SaveChanges();
            return Results.Ok(true);
        }


    }
}
