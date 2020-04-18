using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Services
{
    public class PickPixDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet <PictureTag> PictureTags { get; set; }
        public DbSet <PictureAlbum> PictureAlbums { get; set; }


        private readonly string _dataBasePath;

        public PickPixDbContext(string databasePath)
        {
            _dataBasePath = databasePath;

            //Create database if not there. This will also ensure the data seeding will happen.
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasKey(t => t.TagId);
            modelBuilder.Entity<PictureTag>().HasKey(p => new { p.PictureId,p.TagId});
            modelBuilder.Entity<PictureAlbum>().HasKey(p => new { p.PictureId, p.AlbumId });
            modelBuilder.Entity<PictureAlbum>()
            .HasOne(bc => bc.Picture)
            .WithMany(b => b.PictureAlbums)
            .HasForeignKey(bc => bc.PictureId);
            modelBuilder.Entity<PictureAlbum>()
            .HasOne(bc => bc.Album)
            .WithMany(c => c.PictureAlbums)
            .HasForeignKey(bc => bc.AlbumId);

            modelBuilder.Entity<PictureTag>()
            .HasOne(bc => bc.Picture)
            .WithMany(b => b.PictureTags)
            .HasForeignKey(bc => bc.PictureId);
            modelBuilder.Entity<PictureTag>()
            .HasOne(bc => bc.Tag)
            .WithMany(c => c.PictureTags)
            .HasForeignKey(bc => bc.TagId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"FileName={_dataBasePath}");
        }


    }
}
