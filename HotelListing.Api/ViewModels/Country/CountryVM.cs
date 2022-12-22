using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.ViewModels
{
    public class CountryVM
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
