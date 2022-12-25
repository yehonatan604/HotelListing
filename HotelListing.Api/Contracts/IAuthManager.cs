using HotelListing.Api.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Api.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserVM userVM);
        Task<AuthResponseDTO?> Login(UserLoginVM loginVM);
        Task<string> CreateRefreshToken();
        Task<AuthResponseDTO?> VerifyRefreshToken(AuthResponseDTO request);
    }
}
