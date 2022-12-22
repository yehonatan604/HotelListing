using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data;
using HotelListing.Api.ViewModels;
using AutoMapper;
using HotelListing.Api.ViewModels.Country;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelDbContext _db;
        private readonly IMapper _mapper;

        public CountriesController(IDbContextFactory<HotelDbContext> dbFactory, IMapper mapper)
        {
            _db = dbFactory.CreateDbContext();
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryGetVM>>> GetCountries()
        {
            var countries = await _db.Countries.ToListAsync();
            var records = _mapper.Map<List<CountryGetVM>>(countries);
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryGetDetailsVM>> GetCountry(int id)
        {
            var country = await _db.Countries.Include(c => c.Hotels)
                .FirstOrDefaultAsync(c => c.Id == id);
            var record = _mapper.Map<CountryGetDetailsVM>(country);
            return country == null ? NotFound() : Ok(record);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _db.Entry(country).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CountryVM countryVM)
        {
            var country = _mapper.Map<Country>(countryVM);
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _db.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _db.Countries.Remove(country);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _db.Countries.Any(e => e.Id == id);
        }
    }
}
