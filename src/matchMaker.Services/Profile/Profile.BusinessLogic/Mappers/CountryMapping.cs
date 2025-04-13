using Profile.BusinessLogic.DTOs.Country.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class CountryMapping : AutoMapper.Profile
{
    public CountryMapping()
    {
        CreateMap<Country, CountryResponseDto>().ReverseMap();
    }
}