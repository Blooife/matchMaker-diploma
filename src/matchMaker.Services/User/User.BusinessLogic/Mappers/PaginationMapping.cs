using User.BusinessLogic.DTOs.Response;
using AutoMapper;
using Common.Models;

namespace User.BusinessLogic.Mappers;

public class PaginationMapping : Profile
{
    public PaginationMapping()
    {
        CreateMap<PagedList<UserResponseDto>, PaginationMetadata>()
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext))
            .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious));
    }
}