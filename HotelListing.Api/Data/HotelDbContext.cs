using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Data
{
    public class HotelDbContext: DbContext
    {
        public HotelDbContext(DbContextOptions options): base(options)
        {

        }
    }
}
