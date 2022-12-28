using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Api.ViewModels.Hotel
{
    public class HotelBaseVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
        public double? Rating { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CountryId { get; set; }

    }
}
