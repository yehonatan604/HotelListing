using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data;
using HotelListing.Api.Contracts;
using AutoMapper;
using HotelListing.Api.ViewModels.Hotel;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using HotelListing.Api.ViewModels.Country;
using HotelListing.Api.ViewModels.Page;

namespace HotelListing.Api.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1.0")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _repo;
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<HotelBaseVM>>> GetHotels()
        {
            return Ok(_mapper.Map<List<HotelBaseVM>>(await _repo.GetAllAsync()));
        }

        // GET: api/Countries?StartIndex=0&PageSize=5&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PageResult<HotelBaseVM>>> GetPagedCountries
            ([FromQuery] QueryParameters queryParameters)
        {
            return Ok(await _repo.GetAllAsync<HotelBaseVM>(queryParameters));
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelBaseVM>> GetHotel(int id)
        {
            var hotel = await _repo.GetAsync(id);
            return hotel == null ? NotFound() : Ok(_mapper.Map<HotelBaseVM>(hotel));
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelVM _hotel)
        {
            if (_hotel.Id != id)
            {
                return BadRequest();
            }

            var hotel = await _repo.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            _mapper.Map(_hotel, hotel);

            try
            {
                await _repo.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                return !await _repo.Exists(id) ? NotFound() : BadRequest();
            }
            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(HotelCreateVM _hotel)
        {
            var hotel = _mapper.Map<Hotel>(_hotel);
            await _repo.AddAsync(hotel);
            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int id)
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
