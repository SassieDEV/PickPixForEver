using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Data
{
    public class PickPixDataContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        private readonly string _dataBasePath;

        public PickPixDataContext(string databasePath)
        {
            _dataBasePath = databasePath;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"FileNaem={_dataBasePath}");
        }
    }
}
