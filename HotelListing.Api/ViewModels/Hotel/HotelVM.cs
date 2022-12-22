using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Api.ViewModels.Hotel
{
    public class HotelVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public double Rating { get; set; }
        public int CountryId { get; set; }

    }
}
