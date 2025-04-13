using Match.BusinessLogic.DTOs.Like;
using Match.DataAccess.Models;

namespace Match.BusinessLogic.Mappers;

public class LikeMapping : AutoMapper.Profile
{
    public LikeMapping()
    {
        CreateMap<AddLikeDto, Like>()
            .ForMember(dest=>dest.ProfileId, opt=>opt.MapFrom(src=>src.ProfileId))
            .ForMember(dest=>dest.TargetProfileId, opt=>opt.MapFrom(src=>src.TargetProfileId))
            .ForMember(dest=>dest.IsLike, opt=>opt.MapFrom(src=>src.IsLike));
        
        CreateMap<Like, LikeResponseDto>()
            .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id))
            .ForMember(dest=>dest.ProfileId, opt=>opt.MapFrom(src=>src.ProfileId))
            .ForMember(dest=>dest.TargetProfileId, opt=>opt.MapFrom(src=>src.TargetProfileId))
            .ForMember(dest=>dest.IsLike, opt=>opt.MapFrom(src=>src.IsLike));
    }
}