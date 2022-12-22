using HotelListing.Api.ViewModels.Hotel;

namespace HotelListing.Api.ViewModels.Country
{
    public class CountryGetDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public List<HotelVM> Hotels { get; set; } 
    }
}
