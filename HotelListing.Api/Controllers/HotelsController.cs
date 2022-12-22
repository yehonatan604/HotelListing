using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data;
using HotelListing.Api.Contracts;
using AutoMapper;
using HotelListing.Api.ViewModels.Hotel;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelVM>>> GetHotels()
        {
            return Ok(_mapper.Map<List<HotelVM>>(await _repo.GetAllAsync()));
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelVM>> GetHotel(int id)
        {
            var hotel = await _repo.GetAsync(id);
            return hotel == null ? NotFound() : Ok(_mapper.Map<HotelVM>(hotel));
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
