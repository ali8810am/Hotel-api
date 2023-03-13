using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HotelListing.Configuration
{
    public class HotelConfiguration:IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {

            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Atlas",
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
                });
        }
    }
}
