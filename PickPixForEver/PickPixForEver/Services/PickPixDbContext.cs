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


        private readonly string _dataBasePath;

        public PickPixDbContext(string databasePath)
        {
            _dataBasePath = databasePath;

            //Create database if not there. This will also ensure the data seeding will happen.
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"FileName={_dataBasePath}");
        }


    }
}
