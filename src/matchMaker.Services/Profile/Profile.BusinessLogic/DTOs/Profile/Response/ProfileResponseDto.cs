using Profile.BusinessLogic.DTOs.City.Response;
using Profile.BusinessLogic.DTOs.Country.Response;
using Profile.BusinessLogic.DTOs.Education.Response;
using Profile.BusinessLogic.DTOs.Goal.Response;
using Profile.BusinessLogic.DTOs.Image.Response;
using Profile.BusinessLogic.DTOs.Interest.Response;
using Profile.BusinessLogic.DTOs.Language.Response;
using Common.Constants;

namespace Profile.BusinessLogic.DTOs.Profile.Response;

public class ProfileResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Bio { get; set; }
    public int? Height { get; set; }
    public bool ShowAge { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
    public CityResponseDto City { get; set; }
    public CountryResponseDto Country { get; set; }
    public GoalResponseDto? Goal { get; set; }
    public string UserId { get; set; }
    public List<LanguageResponseDto> Languages { get; set; } = new ();
    public List<InterestResponseDto> Interests { get; set; } = new();
    public List<ProfileEducationResponseDto> Education { get; set; } = new();
    public List<ImageResponseDto> Images { get; set; } = new ();
}