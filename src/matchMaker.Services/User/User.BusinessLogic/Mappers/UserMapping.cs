using User.BusinessLogic.DTOs.Response;
using AutoMapper;
using User.BusinessLogic.DTOs.Request;

namespace User.BusinessLogic.Mappers;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<UserRequestDto, User.DataAccess.Models.User>()
            .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.Email))
            .ForMember(dest => dest.NormalizedEmail, act => act.MapFrom(src => src.Email.ToUpper()));
        
        CreateMap<User.DataAccess.Models.User, UserResponseDto>()
            .ForMember(dest => dest.Roles, act => act.MapFrom(src => src.Roles));
        

        CreateMap<User.DataAccess.Models.User, LoginResponseDto>();
    }
}