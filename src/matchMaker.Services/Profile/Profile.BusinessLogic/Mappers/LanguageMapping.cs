using Profile.BusinessLogic.DTOs.Language.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class LanguageMapping : AutoMapper.Profile
{
    public LanguageMapping()
    {
        CreateMap<Language, LanguageResponseDto>().ReverseMap();
    }
}