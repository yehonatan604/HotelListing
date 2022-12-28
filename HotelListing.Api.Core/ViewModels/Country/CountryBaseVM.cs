using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.ViewModels.Country
{
    public abstract class CountryBaseVM
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
