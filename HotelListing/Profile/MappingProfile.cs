using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;

namespace HotelListing.Profile
{
    public class MappingProfile:AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<City, CreateCityDto>().ReverseMap();
        }
    }
}
