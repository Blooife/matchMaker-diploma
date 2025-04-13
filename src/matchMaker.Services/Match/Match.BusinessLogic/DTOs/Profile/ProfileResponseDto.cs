using Common.Constants;

namespace Match.BusinessLogic.DTOs.Profile;

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
    public CountryResponseDto Country { get; set; }
    public CityResponseDto City { get; set; }
    public GoalResponseDto? Goal { get; set; }
    public List<LanguageResponseDto> Languages { get; set; } = new ();
    public List<InterestResponseDto> Interests { get; set; } = new ();
    public List<ImageResponseDto> Images { get; set; } = new ();
    public List<ProfileEducationResponseDto> Education { get; set; } = new ();
}

public class CityResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CountryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class ProfileEducationResponseDto
{
    public string ProfileId { get; set; }
    public int EducationId { get; set; }
    public string EducationName { get; set; }
    public string Description { get; set; }
}

public class GoalResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class ImageResponseDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
}

public class InterestResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class LanguageResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}