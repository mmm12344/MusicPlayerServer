using Microsoft.AspNetCore.Mvc;
using MusicPlayerServer.Models;
using NuGet.Protocol;
using System.Security.Cryptography.Xml;

namespace MusicPlayerServer
{
    public class RequestHandlers
    {
        public static async Task<IResult> GetLikedSongs()
        {
            IQueryable query;
            await using (var context = new MusicPlayerServerContext())
            {
                query = from userLikes
                        in context.UserLikes
                        where userLikes.UserID == 1
                        select new 
                        { 
                            userLikes.SongID ,
                            userLikes.Song.Name 
                        };
            }
            return Results.Ok(query);
        }

        public static async Task<IResult> GetOwnSongs()
        {
            IQueryable query;
            await using(var context = new MusicPlayerServerContext())
            {
                query = from songs
                        in context.Songs
                        where songs.UserID == 1
                        select new
                        {
                            songs.SongID,
                            songs.Name
                        };
            }
            return Results.Ok(query);
        }

        public static async Task<IResult> GetAllSongs()
        {
            IQueryable query;
            await using(var context = new MusicPlayerServerContext())
            {
                query = from songs
                        in context.Songs
                        select new
                        {
                            songs.SongID,
                            songs.Name
                        };
            }
            return Results.Ok(query);
        }

        public static async Task<IResult> Search(string songName)
        {
            string[] songKeyWords = songName.Split(" ");
            IQueryable query;
            await using(var context = new MusicPlayerServerContext())
            {
                query = from songs
                        in context.Songs
                        where songKeyWords.Contains(songName)
                        select new
                        {
                            songs.SongID,
                            songs.Name
                        };
            }
            return Results.Ok(query);
        }

        public static async Task<IResult> GetAllPlaylists()
        {
            IQueryable query;
            await using(var context = new MusicPlayerServerContext())
            {
                query = from playlist
                        in context.Playlists
                        where playlist.UserID == 1
                        select new
                        {
                            playlist.PlaylistID,
                            playlist.Name
                        };
            }
            return Results.Ok(query);
        }

        public static async Task<IResult> GetPlayListSongs(int playlistID)
        {
            IQueryable query;
            await using(var context = new MusicPlayerServerContext())
            {
                query = from songPlaylist
                        in context.SongPlaylists
                        where songPlaylist.PlaylistID == playlistID
                        select new
                        {
                            songPlaylist.SongID,
                            songPlaylist.Song.Name
                        };
            }
            return Results.Ok(query);
        }

        public static async Task<IResult> AddSong(Song song)
        {
            await using(var context = new MusicPlayerServerContext())
            {
                await context.Songs.AddAsync(song);
                context.SaveChanges();
            }
            return Results.Ok();
        }

        public static async Task<IResult> AddPlaylist(Playlist playlist)
        {
            await using(var context = new MusicPlayerServerContext())
            {
                await context.Playlists.AddAsync(playlist);
                context.SaveChanges();
            }
            return Results.Ok();
        }

        public static async Task<IResult> AddSongToPlaylist(SongPlaylist songPlaylist)
        {
            await using(var context = new MusicPlayerServerContext())
            {
                await context.SongPlaylists.AddAsync(songPlaylist);
                context.SaveChanges();
            }
            return Results.Ok();
        }

        public static async Task<IResult> DeleteSong(int songID)
        {
            await using(var context = new MusicPlayerServerContext())
            {
                var query = from song
                            in context.Songs
                            where song.SongID == songID
                            select song;
                context.Songs.Remove(query.FirstOrDefault());
                context.SaveChanges();
            }
            return Results.Ok();
        }

        public static async Task<IResult> AddLikeToSong(int songID)
        {
            await using(var context = new MusicPlayerServerContext())
            {
                UserLikes entry = new UserLikes { SongID = songID, UserID = 1 };
                context.UserLikes.Add(entry);
                context.SaveChanges();
            }
            return Results.Ok();
        }

        public static async Task<IResult> GetSong(int songID)
        {
            IQueryable query;
            await using(var context = new MusicPlayerServerContext())
            {
                query = from song
                        in context.Songs
                        where song.SongID == songID
                        select song;
            }
            return Results.Ok(query);
        }


    }
}
