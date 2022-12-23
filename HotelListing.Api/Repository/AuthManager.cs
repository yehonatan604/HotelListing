using AutoMapper;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Api.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public AuthManager(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> Login(UserLoginVM loginVM)
        {
            bool isValid;
            try
            {

                var user = await _userManager.FindByEmailAsync(loginVM.Email);
                if (user == null) 
                { 
                    return default; 
                }
                
                isValid = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (!isValid) 
                {
                    return default;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isValid;
        }

        public async Task<IEnumerable<IdentityError>> Register(UserVM userVM)
        {
            var user = _mapper.Map<User>(userVM);
            user.UserName = userVM.Email;
            var result = await _userManager.CreateAsync(user, userVM.Password);

            if (result.Succeeded)
            {
                await _userManager.UpdateAsync(user);
            }

            return result.Errors;
        }
    }
}
