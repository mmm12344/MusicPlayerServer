using Microsoft.AspNetCore.Mvc;
using MusicPlayerServer.Models;
using NuGet.Protocol;
using System.Security.Cryptography.Xml;

namespace MusicPlayerServer
{
    public class RequestHandlers
    {
        private static MusicPlayerServerContext context = new MusicPlayerServerContext();
        public static async Task<IResult> GetLikedSongs()
        {
            //var context = new MusicPlayerServerContext();
            var query = from userLikes
                        in context.UserLikes
                        where userLikes.UserID == 1
                        select new 
                        { 
                            userLikes.SongID ,
                            userLikes.Song.Name 
                        };
            return Results.Ok(query);
        }

        public static async Task<IResult> GetOwnSongs()
        {
            
            //var context = new MusicPlayerServerContext();
            var query = from songs
                    in context.Songs
                    where songs.UserID == 1
                    select new
                    {
                        songs.SongID,
                        songs.Name
                    };
            return Results.Ok(query);
        }

        public static async Task<IResult> GetAllSongs()
        {
            //var context = new MusicPlayerServerContext();
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
            string[] songKeyWords = songName.Split(" ");
            //var context = new MusicPlayerServerContext();
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

        public static async Task<IResult> GetAllPlaylists()
        {
            //var context = new MusicPlayerServerContext();

            var query = from playlist
                    in context.Playlists
                    where playlist.UserID == 1
                    select new
                    {
                        playlist.PlaylistID,
                        playlist.Name
                    };

            return Results.Ok(query);
        }

        public static async Task<IResult> GetPlayListSongs(int playlistID)
        {
            //var context = new MusicPlayerServerContext();

            var query = from songPlaylist
                        in context.SongPlaylists
                        where songPlaylist.PlaylistID == playlistID
                        select new
                        {
                            songPlaylist.SongID,
                            songPlaylist.Song.Name
                        };

            return Results.Ok(query);
        }

        public static async Task<IResult> AddSong(Song song)
        {
            //var context = new MusicPlayerServerContext();

            context.Songs.AddAsync(song);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> AddPlaylist(Playlist playlist)
        {
            //var context = new MusicPlayerServerContext();

            await context.Playlists.AddAsync(playlist);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> AddSongToPlaylist(SongPlaylist songPlaylist)
        {
            //var context = new MusicPlayerServerContext();

            await context.SongPlaylists.AddAsync(songPlaylist);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> DeleteSong(int songID)
        {
            //var context = new MusicPlayerServerContext();

            var query = from song
                        in context.Songs
                        where song.SongID == songID
                        select song;
            context.Songs.Remove(query.FirstOrDefault());
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> AddLikeToSong(int songID)
        {
            //var context = new MusicPlayerServerContext();

            UserLikes entry = new UserLikes { SongID = songID, UserID = 1 };
            context.UserLikes.Add(entry);
            context.SaveChanges();

            return Results.Ok();
        }

        public static async Task<IResult> GetSong(int songID)
        {

            //var context = new MusicPlayerServerContext();

            var query = from song
                    in context.Songs
                    where song.SongID == songID
                    select song;

            return Results.Ok(query);
        }


    }
}
