using Profile.BusinessLogic.DTOs.Education.Request;
using Profile.BusinessLogic.DTOs.Education.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class EducationMapping : AutoMapper.Profile
{
    public EducationMapping()
    {
        CreateMap<AddOrRemoveProfileEducationDto, ProfileEducation>();
        
        CreateMap<Education, EducationResponseDto>();
        
        CreateMap<ProfileEducation, ProfileEducationResponseDto>()
            .ForMember(dest=>dest.Description, opt => opt.MapFrom(src=>src.Description))
            .ForMember(dest=>dest.ProfileId, opt => opt.MapFrom(src=>src.ProfileId))
            .ForMember(dest=>dest.EducationId, opt => opt.MapFrom(src=>src.EducationId))
            .ForMember(dest=>dest.EducationName, opt => opt.MapFrom(src=>src.Education.Name));

        CreateMap<ProfileEducationResponseDto, ProfileEducation>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId))
            .ForPath(dest => dest.Education.Name, opt => opt.MapFrom(src => src.EducationName))
            .ForPath(dest => dest.Education.Id, opt => opt.MapFrom(src => src.EducationId))
            .ForMember(dest => dest.EducationId, opt => opt.MapFrom(src => src.EducationId));
    }
}