using HotelListing.Api.Configurations.ModelConfigs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Data
{
    public class HotelDbContext : IdentityDbContext<User>
    {
        public HotelDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new CountryConfig());
            builder.ApplyConfiguration(new HotelConfig());
        }
    }
}
