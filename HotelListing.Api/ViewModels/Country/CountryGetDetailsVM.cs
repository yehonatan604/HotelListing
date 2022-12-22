using HotelListing.Api.ViewModels.Hotel;

namespace HotelListing.Api.ViewModels.Country
{
    public class CountryGetDetailsVM :CountryBaseVM
    {
        public int Id { get; set; }

        public List<HotelBaseVM> Hotels { get; set; } 
    }
}
