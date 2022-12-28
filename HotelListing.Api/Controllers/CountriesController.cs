using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data;
using AutoMapper;
using HotelListing.Api.ViewModels.Country;
using HotelListing.Api.Contracts;
using Microsoft.AspNetCore.Authorization;
using HotelListing.Api.Exceptions;
using HotelListing.Api.ViewModels.Page;
using Microsoft.AspNetCore.OData.Query;

namespace HotelListing.Api.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _repo;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(
            IMapper mapper,
            ICountriesRepository countriesRepo,
            ILogger<CountriesController> logger)
        {
            _mapper = mapper;
            _repo = countriesRepo;
            _logger = logger;
        }

        // GET: api/Countries/GetAll
        [HttpGet("GetAll")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<CountryGetVM>>> GetCountries()
        {
            return Ok(_mapper.Map<List<CountryGetVM>>(await _repo.GetAllAsync()));
        }
        
        // GET: api/Countries?StartIndex=0&PageSize=5&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PageResult<CountryGetVM>>> GetPagedCountries
            ([FromQuery] QueryParameters queryParameters)
        {
            return Ok(await _repo.GetAllAsync<CountryGetVM>(queryParameters));
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryGetDetailsVM>> GetCountry(int id)
        {
            var country = await _repo.GetDetails(id);
            return country == null ?
                   throw new NotFoundException(nameof(GetCountry), id) :
                   Ok(_mapper.Map<CountryGetDetailsVM>(country));
        }

        // PUT: api/Countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, CountryUpdateVM countryUpdateVM)
        {
            if (countryUpdateVM.Id != id)
            {
                throw new BadRequestException(nameof(PutCountry), id);
            }

            var country = await _repo.GetAsync(id);

            _ = country == null ?
                           throw new NotFoundException(nameof(PutCountry), id) :
                           _mapper.Map(countryUpdateVM, country);

            await _repo.UpdateAsync(country);
            return Ok(country);
        }

        // POST: api/Countries
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CountryCreateVM countryVM)
        {
            var country = _mapper.Map<Country>(countryVM);
            await _repo.AddAsync(country);
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (await _repo.GetAsync(id) == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
