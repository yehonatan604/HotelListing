using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.ViewModels.User
{
    public class UserVM : UserLoginVM
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
