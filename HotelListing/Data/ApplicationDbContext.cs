using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasData(
                new City
                {
                    Id = 1,
                    Name = "Tehran",
                    State = "Tehran"
                },
            new City
            {
                Id = 2,
                Name = "Mashhad",
                State = "SouthKhorasan"
            },
                new City
                {
                    Id = 3,
                    Name = "Esfehan",
                    State = "Esfehan"
                }
            );
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name ="Atlas",
                    Address = "addrres",
                    CityId = 1,
                    Description = "sss",
                    Rating = 3
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Homa",
                    Address = "addrres",
                    CityId = 1,
                    Description = "sss",
                    Rating = 4
                }
                );
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

    }
}
