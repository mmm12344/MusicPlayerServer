

using MusicPlayerServer;
using MusicPlayerServer.Models;
using Microsoft.AspNetCore.OpenApi;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MusicPlayerServerContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World");

var authenticationGroup = app.MapGroup("/authentication");

authenticationGroup.MapPost("/signup", Authentication.SignUp);
authenticationGroup.MapPost("/signin", Authentication.SignIn);

var apiGroup = app.MapGroup("/api").AddEndpointFilter(Authorization.RequiresSignIn);

apiGroup.MapGet("/get_liked_songs", RequestHandlers.GetLikedSongs);
apiGroup.MapGet("/get_own_songs", RequestHandlers.GetOwnSongs);
apiGroup.MapGet("/get_all_songs", RequestHandlers.GetAllSongs);
apiGroup.MapGet("/search/{songName}", RequestHandlers.Search);
apiGroup.MapGet("/get_all_playlists", RequestHandlers.GetAllPlaylists);
apiGroup.MapGet("/get_playlist_songs/{playlistID}", RequestHandlers.GetPlayListSongs);
apiGroup.MapGet("/delete_song/{songID}", RequestHandlers.DeleteSong);
apiGroup.MapGet("/add_like_to_song/{songID}", RequestHandlers.AddLikeToSong);
apiGroup.MapGet("/get_song/{songID}", RequestHandlers.GetSong);

apiGroup.MapPost("/add_song", RequestHandlers.AddSong);
apiGroup.MapPost("/add_playlist", RequestHandlers.AddPlaylist);
apiGroup.MapPost("/add_song_to_playlist", RequestHandlers.AddSongToPlaylist);


app.Run();
