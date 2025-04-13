using Profile.BusinessLogic.DTOs.Interest.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class InterestMapping : AutoMapper.Profile
{
    public InterestMapping()
    {
        CreateMap<Interest, InterestResponseDto>().ReverseMap();
    }
}