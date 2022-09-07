using KonjeticZavrsni.Model;
using Microsoft.EntityFrameworkCore;

namespace KonjeticZavrsni.DAL
{
    public class DriverManagerDbContext : DbContext
    {
        public DriverManagerDbContext(DbContextOptions<DriverManagerDbContext> options) : base(options) { }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<RaceTrack> RaceTracks { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>()

            modelBuilder.Entity<RaceTrack>().HasData(new RaceTrack { Id = 1, Name = "Catalunya"});
            modelBuilder.Entity<RaceTrack>().HasData(new RaceTrack { Id = 2, Name = "Circuit de Monaco"});
            modelBuilder.Entity<RaceTrack>().HasData(new RaceTrack { Id = 3, Name = "Hockenheim" });

            modelBuilder.Entity<Country>().HasData(new Country { Id = 1, Name = "Španjolska" });
            modelBuilder.Entity<Country>().HasData(new Country { Id = 2, Name = "Monaco" });
            modelBuilder.Entity<Country>().HasData(new Country { Id = 3, Name = "Njemačka" });

           
        }

    }
}