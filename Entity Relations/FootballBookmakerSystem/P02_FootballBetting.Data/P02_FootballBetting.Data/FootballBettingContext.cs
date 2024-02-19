using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using P02_FootballBetting.Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        private const string ConnectionString = "Server=DESKTOP-M5SEPFK\\SQLEXPRESS;Database=FootballBetting;Integrated Security=True;TrustServerCertificate=True;";


        public FootballBettingContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Bet> Bets { get; set; }    
        public DbSet<PlayerStatistic>  PlayersStatistics { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.PlayerId, ps.GameId });
        }
    }

}