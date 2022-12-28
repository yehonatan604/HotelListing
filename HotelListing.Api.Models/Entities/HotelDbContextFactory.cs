using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelListing.Api.Data
{
    public class HotelDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
    {
        public HotelDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            DbContextOptionsBuilder<HotelDbContext> optionsBuilder = new();
            var conn = config.GetConnectionString("HotelListingDbCnnectionString");
            optionsBuilder.UseSqlServer(conn);

            return new HotelDbContext(optionsBuilder.Options);
        }
    }
}
