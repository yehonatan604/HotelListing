using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.ViewModels.User
{
    public class UserLoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1} charcters!!!",
            MinimumLength = 6)]
        public string Password { get; set; }
    }
}
