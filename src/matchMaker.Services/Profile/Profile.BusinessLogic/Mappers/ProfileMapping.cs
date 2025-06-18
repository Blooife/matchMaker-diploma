using Common.Dtos.Profile;
using MessageQueue.Messages.Profile;
using Profile.BusinessLogic.DTOs.Profile.Request;
using Profile.BusinessLogic.DTOs.Profile.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class ProfileMapping : AutoMapper.Profile
{
    public ProfileMapping()
    {
        CreateMap<UserProfile, ProfileResponseDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests))
            .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.ProfileEducations))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)).ReverseMap();
        
        CreateMap<CreateOrUpdateProfileDto, UserProfile>();

        //CreateMap<UserCreatedMessage, CreateOrUpdateUserDto>();

        CreateMap<UserProfile, ProfileCreatedEventMessage>()
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.City.CountryId))
            .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0].ImageUrl : null));
        
        CreateMap<UserProfile, ProfileUpdatedEventMessage>()
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.City.CountryId))
            .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images[0].ImageUrl : null));
        
        CreateMap<UserProfile, ProfileClientDto>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests))
            .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.ProfileEducations));

        CreateMap<City, CityClientDto>();
        CreateMap<Country, CountryClientDto>();
        CreateMap<Goal, GoalClientDto>();
        CreateMap<Language, LanguageClientDto>();
        CreateMap<Interest, InterestClientDto>();
        CreateMap<ProfileEducation, ProfileEducationClientDto>();
        CreateMap<Image, ImageClientDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
    }
}