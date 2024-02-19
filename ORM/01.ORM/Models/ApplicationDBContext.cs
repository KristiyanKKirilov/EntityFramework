using _01.ORM.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.ORM.Models
{
    public class ApplicationDBContext : DbContext
    {
        private const string connectionString =
            @"Server=DESKTOP-M5SEPFK\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
