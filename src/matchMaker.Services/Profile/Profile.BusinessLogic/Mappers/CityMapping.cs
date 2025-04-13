using Profile.BusinessLogic.DTOs.City.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class CityMapping : AutoMapper.Profile
{
    public CityMapping()
    {
        CreateMap<City, CityResponseDto>().ReverseMap();

        CreateMap<City, CityWithCountryResponseDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country));
    }
}