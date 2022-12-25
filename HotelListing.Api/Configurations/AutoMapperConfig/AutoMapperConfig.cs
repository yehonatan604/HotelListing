using AutoMapper;
using HotelListing.Api.Data;
using HotelListing.Api.ViewModels.Country;
using HotelListing.Api.ViewModels.Hotel;
using HotelListing.Api.ViewModels.User;

namespace HotelListing.Api.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Country, CountryBaseVM>().ReverseMap();
            CreateMap<Country, CountryGetVM>().ReverseMap();
            CreateMap<Country, CountryGetDetailsVM>().ReverseMap();
            CreateMap<Country, CountryUpdateVM>().ReverseMap();
            CreateMap<Country, CountryCreateVM>().ReverseMap();

            CreateMap<Hotel, HotelVM>().ReverseMap();
            CreateMap<Hotel, HotelCreateVM>().ReverseMap();

            CreateMap<UserVM, User>().ReverseMap();
        }
    }
}
