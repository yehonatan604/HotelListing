using HotelListing.Api.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Api.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserVM userVM);
    }
}
