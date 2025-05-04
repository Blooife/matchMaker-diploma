using User.BusinessLogic.DTOs.Response;
using AutoMapper;
using User.DataAccess.Models;

namespace User.BusinessLogic.Mappers;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<string, Role>()
            .ForMember(dest => dest.Name, act=> act.MapFrom(src => src))
            .ForMember(dest => dest.NormalizedName, act=> act.MapFrom(src => src.ToUpper()));
        
        CreateMap<Role, string>().ConvertUsing(src => src.Name);

        CreateMap<Role, RoleResponseDto>();
    }
}