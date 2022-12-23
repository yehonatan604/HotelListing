using HotelListing.Api.Contracts;
using HotelListing.Api.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _manager;
        public AuthController(IAuthManager manager)
        {
            _manager = manager;
        }


        // POST: api/Auth/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody]UserVM userVM)
        {
            var errors = await _manager.Register(userVM);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok(userVM); //it will send all the details including password back to ttte client!!!!
        }
        
        // POST: api/Auth/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody]UserLoginVM loginVM)
        {
            var isValidUser = await _manager.Login(loginVM);

            if (!isValidUser)
            {
                return Unauthorized(); // error 401
            }
            return Ok();
        }
    }
}
