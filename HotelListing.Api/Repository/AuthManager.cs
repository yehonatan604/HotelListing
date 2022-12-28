using AutoMapper;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.ViewModels.Auth;
using HotelListing.Api.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListing.Api.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuthManager> _logger;
        private readonly UserManager<User> _manager;
        private readonly IConfiguration _config;
        private User _user;

        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(
            IMapper mapper,
            UserManager<User> userManager,
            IConfiguration config,
            ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _manager = userManager;
            _config = config;
        }

        public async Task<string> CreateRefreshToken()
        {
            await _manager.RemoveAuthenticationTokenAsync
                (_user, _loginProvider, _refreshToken);
            var newRefreshToken = await _manager.GenerateUserTokenAsync
                (_user, _loginProvider, _refreshToken);
            var result = await _manager.SetAuthenticationTokenAsync
                (_user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDTO?> Login(UserLoginVM loginVM)
        {
            _logger.LogInformation($"Looking for user '{loginVM.Email}'");
            _user = await _manager.FindByEmailAsync(loginVM.Email);
            bool isValid = await _manager.CheckPasswordAsync(_user, loginVM.Password);

            if (_user == null || !isValid)
            {
                _logger.LogWarning($"user with email '{loginVM.Email}' was not found!!!");
                return null;
            }

            var token = await GenerateToken();
            _logger.LogInformation($"Generated token: {token}\nfor user: {loginVM.Email}");

            return new AuthResponseDTO
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(UserVM userVM)
        {
            var user = _mapper.Map<User>(userVM);
            user.UserName = userVM.Email;
            var result = await _manager.CreateAsync(user, userVM.Password);

            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }

        public async Task<AuthResponseDTO?> VerifyRefreshToken(AuthResponseDTO request)
        {
            var jsonSecurityTokeHandler = new JwtSecurityTokenHandler();
            var tokenContent = jsonSecurityTokeHandler.ReadJwtToken(request.Token);
            var userName = tokenContent.Claims.ToList()
                .FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Email)?.Value;

            _user = await _manager.FindByNameAsync(userName);

            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _manager.VerifyUserTokenAsync
                (_user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDTO
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }
            await _manager.UpdateSecurityStampAsync(_user);
            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Keys:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _manager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _manager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToInt16(_config["JwtSettings:DurationInDays"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
