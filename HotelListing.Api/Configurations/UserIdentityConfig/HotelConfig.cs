using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Api.Configurations.UserIdentityConfig
{
    public class HotelConfig : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Veronanto",
                    Adress = "Varsha",
                    CountryId = 3,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Esperanto",
                    Adress = "Negril",
                    CountryId = 2,
                    Rating = 3.8
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Kokama",
                    Adress = "Negril",
                    CountryId = 2,
                    Rating = 4.8
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Zivonardo",
                    Adress = "Tel-Aviv",
                    CountryId = 1,
                    Rating = 3.6
                });
        }
    }
}
