﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicPlayerServer.Models;

#nullable disable

namespace MusicPlayerServer.Migrations
{
    [DbContext(typeof(MusicPlayerServerContext))]
    [Migration("20240103235257_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.14");

            modelBuilder.Entity("MusicPlayerServer.Models.Playlist", b =>
                {
                    b.Property<int>("PlaylistID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Picture")
                        .HasColumnType("BLOB");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlaylistID");

                    b.HasIndex("UserID");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.Song", b =>
                {
                    b.Property<int>("SongID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("File")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int?>("Likes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Picture")
                        .HasColumnType("BLOB");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("SongID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserID");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.SongPlaylist", b =>
                {
                    b.Property<int>("SongPlaylistID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlaylistID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SongID")
                        .HasColumnType("INTEGER");

                    b.HasKey("SongPlaylistID");

                    b.HasIndex("PlaylistID");

                    b.HasIndex("SongID");

                    b.ToTable("SongPlaylists");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.UserLikes", b =>
                {
                    b.Property<int>("UserLikesID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SongID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserLikesID");

                    b.HasIndex("SongID");

                    b.HasIndex("UserID");

                    b.ToTable("UserLikes");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.Playlist", b =>
                {
                    b.HasOne("MusicPlayerServer.Models.User", "User")
                        .WithMany("Playlists")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.Song", b =>
                {
                    b.HasOne("MusicPlayerServer.Models.User", "User")
                        .WithMany("Songs")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.SongPlaylist", b =>
                {
                    b.HasOne("MusicPlayerServer.Models.Playlist", "Playlist")
                        .WithMany("SongPlaylists")
                        .HasForeignKey("PlaylistID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicPlayerServer.Models.Song", "Song")
                        .WithMany("SongPlaylists")
                        .HasForeignKey("SongID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.UserLikes", b =>
                {
                    b.HasOne("MusicPlayerServer.Models.Song", "Song")
                        .WithMany("LikedByUsers")
                        .HasForeignKey("SongID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicPlayerServer.Models.User", "User")
                        .WithMany("LikedSongs")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.Playlist", b =>
                {
                    b.Navigation("SongPlaylists");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.Song", b =>
                {
                    b.Navigation("LikedByUsers");

                    b.Navigation("SongPlaylists");
                });

            modelBuilder.Entity("MusicPlayerServer.Models.User", b =>
                {
                    b.Navigation("LikedSongs");

                    b.Navigation("Playlists");

                    b.Navigation("Songs");
                });
#pragma warning restore 612, 618
        }
    }
}
