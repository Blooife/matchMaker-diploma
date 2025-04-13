using Common.Models;
using Match.BusinessLogic.DTOs.Chat;
using Match.BusinessLogic.DTOs.Match;
using Match.BusinessLogic.DTOs.Message;
using Match.BusinessLogic.DTOs.Profile;

namespace Match.BusinessLogic.Mappers;

public class PaginationMapping : AutoMapper.Profile
{
    public PaginationMapping()
    {
        CreateMap<PagedList<ChatResponseDto>, PaginationMetadata>()
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext))
            .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious));
        
        CreateMap<PagedList<MessageResponseDto>, PaginationMetadata>()
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext))
            .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious));
        
        CreateMap<PagedList<MatchResponseDto>, PaginationMetadata>()
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext))
            .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious));
        
        CreateMap<PagedList<ProfileResponseDto>, PaginationMetadata>()
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
            .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext))
            .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious));
    }
}