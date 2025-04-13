using Match.BusinessLogic.DTOs.Match;
using Match.DataAccess.Models;

namespace Match.BusinessLogic.Mappers;

public class MatchMapping : AutoMapper.Profile
{
    public MatchMapping()
    {
        CreateMap<MatchEntity, MatchResponseDto>()
            .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id))
            .ForMember(dest=>dest.FirstProfileId, opt=>opt.MapFrom(src=>src.FirstProfileId))
            .ForMember(dest=>dest.SecondProfileId, opt=>opt.MapFrom(src=>src.SecondProfileId));
    }
}