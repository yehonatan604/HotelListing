﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data;
using HotelListing.Api.ViewModels;
using AutoMapper;
using HotelListing.Api.ViewModels.Country;
using HotelListing.Api.Contracts;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _repo;

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepo)
        {
            _mapper = mapper;
            _repo = countriesRepo;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryGetVM>>> GetCountries()
        {
            var countries = await _repo.GetAllAsync();
            return Ok(_mapper.Map<List<CountryGetVM>>(countries));
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryGetDetailsVM>> GetCountry(int id)
        {
            var country = await _repo.GetDetails(id);
            return country == null ? NotFound() : Ok(_mapper.Map<CountryGetDetailsVM>(country));
        }

        // PUT: api/Countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, CountryUpdateVM countryUpdateVM)
        {
            var country = await _repo.GetAsync(id);

            if (country == null)
            {
                return BadRequest();
            }

            _mapper.Map(countryUpdateVM, country);

            try
            {
                await _repo.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                return !await _repo.Exists(id) ? NotFound() : BadRequest();
            }
            return NoContent();
        }

        // POST: api/Countries
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CountryVM countryVM)
        {
            var country = _mapper.Map<Country>(countryVM);
            await _repo.AddAsync(country);
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (await _repo.GetAsync(id) == null)
            {
                return NotFound();
            }

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
