using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configuration
{
    public class CityConfiguration:IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasData(
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
        }
    }
}
