using HotelListing.Api.Contracts;
using HotelListing.Api.Exceptions;
using HotelListing.Api.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _manager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthManager manager, ILogger<AuthController> logger)
        {
            _manager = manager;
            _logger = logger;
        }


        // POST: api/Auth/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] UserVM userVM)
        {
            _logger.LogInformation($"Registration attempt for {userVM.Email}");

            var errors = await _manager.Register(userVM);
            
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new BadRequestException(nameof(Register), ModelState);
            }
            return Ok();
        }

        // POST: api/Auth/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] UserLoginVM loginVM)
        {
            _logger.LogInformation($"Login attempt for {loginVM.Email}");
            var authResponse = await _manager.Login(loginVM);
            return authResponse == null ?
                   throw new UnauthorizedException(nameof(Login), loginVM.Email) : 
                   Ok(authResponse);
        }

        // POST: api/Auth/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDTO request)
        {
            var authResponse = await _manager.VerifyRefreshToken(request);
            return authResponse == null ?
                   throw new UnauthorizedException(nameof(RefreshToken), request.UserId!) :
                   Ok(authResponse);
        }
    }
}
