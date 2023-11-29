using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerServer.Models;
using NuGet.Protocol;
using System.Net.Http;
using System.Security.Cryptography.Xml;

namespace MusicPlayerServer
{
    public record AddSongRecord(string name, byte[] file, byte[] picture);
    public record AddPlaylistRecord(string name);
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
                            userLikes.Song.Name 
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
                        songs.Name
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
                        songs.Name
                    };
            return Results.Ok(query);
        }

        public static async Task<IResult> Search(string songName)
        {
            string[] songKeyWords = songName.Split("_");
            
            var query = from songs
                    in context.Songs
                    where songKeyWords.Contains(songName)
                    select new
                    {
                        songs.SongID,
                        songs.Name
                    };
            return Results.Ok(query);
        }

        public static async Task<IResult> GetAllPlaylists(HttpContext httpContext)
        {
            
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));

            var query = from playlist
                    in context.Playlists
                    where playlist.UserID == userID
                    select new
                    {
                        playlist.PlaylistID,
                        playlist.Name
                    };

            return Results.Ok(query);
        }

        public static async Task<IResult> GetPlayListSongs(int playlistID, HttpContext httpContext)
        {
            
            int userID = Convert.ToInt32(Authorization.GetCookie("userID", httpContext));

            var query = from songPlaylist
                        in context.SongPlaylists
                        where songPlaylist.PlaylistID == playlistID && songPlaylist.Playlist.UserID == userID
                        select new
                        {
                            songPlaylist.SongID,
                            songPlaylist.Song.Name
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
            Playlist playlist = new Playlist { Name = playlistToAdd.name, UserID = userID };

            await context.Playlists.AddAsync(playlist);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> AddSongToPlaylist(SongPlaylist songPlaylist)
        {


            await context.SongPlaylists.AddAsync(songPlaylist);
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
                    select song;

            return Results.Ok(query);
        }


    }
}
