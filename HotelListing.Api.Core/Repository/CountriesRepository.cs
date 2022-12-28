using AutoMapper;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelDbContext _db;
        public CountriesRepository
            (IDbContextFactory<HotelDbContext> dbFactory, IMapper mapper) : base(dbFactory, mapper)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<Country> GetDetails(int id)
        {
           return (await _db.Countries.Include(c => c.Hotels)
                .FirstOrDefaultAsync(c => c.Id == id))!;
        }
    }
}
