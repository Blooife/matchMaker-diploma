using Common.Dtos.Profile;
using Match.BusinessLogic.DTOs.Profile;
using MessageQueue.Messages.Profile;

namespace Match.BusinessLogic.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<CreateProfileDto, DataAccess.Models.Profile>();
        CreateMap<UpdateProfileDto, DataAccess.Models.Profile>();

        CreateMap<ProfileCreatedEventMessage, DataAccess.Models.Profile>();
        CreateMap<ProfileUpdatedEventMessage, DataAccess.Models.Profile>();
        
        CreateMap<ProfileClientDto, ProfileResponseDto>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests))
            .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education));

        CreateMap<CityClientDto, CityResponseDto>();
        
        CreateMap<CountryClientDto, CountryResponseDto>();
        
        CreateMap<GoalClientDto, GoalResponseDto>();
        
        CreateMap<LanguageClientDto, LanguageResponseDto>();
        
        CreateMap<InterestClientDto, InterestResponseDto>();
        
        CreateMap<ProfileEducationClientDto, ProfileEducationResponseDto>();
        
        CreateMap<ImageClientDto, ImageResponseDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

    }
}