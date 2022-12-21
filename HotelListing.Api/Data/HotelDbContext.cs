using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Israel",
                    ShortName = "IL"
                },
                new Country
                {
                    Id = 2,
                    Name = "Jamaica",
                    ShortName = "JM"
                }, 
                new Country
                {
                    Id = 3,
                    Name = "Poland",
                    ShortName = "PL"
                });

            builder.Entity<Hotel>().HasData(
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
                }
            );
        }
    }
}
