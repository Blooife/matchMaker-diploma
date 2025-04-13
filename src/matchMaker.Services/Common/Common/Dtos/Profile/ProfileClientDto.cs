using Common.Constants;

namespace Common.Dtos.Profile;

public class ProfileClientDto
{
    public long Id { get; set; }
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
    public CountryClientDto Country { get; set; }
    public CityClientDto City { get; set; }
    public GoalClientDto? Goal { get; set; }
    public List<LanguageClientDto> Languages { get; set; } = new ();
    public List<InterestClientDto> Interests { get; set; } = new ();
    public List<ImageClientDto> Images { get; set; } = new ();
    public List<ProfileEducationClientDto> Education { get; set; } = new ();
}

public class CityClientDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class CountryClientDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class ProfileEducationClientDto
{
    public long ProfileId { get; set; }
    public long EducationId { get; set; }
    public string EducationName { get; set; }
    public string Description { get; set; }
}

public class GoalClientDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class ImageClientDto
{
    public long Id { get; set; }
    public string ImageUrl { get; set; }
}

public class InterestClientDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class LanguageClientDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}