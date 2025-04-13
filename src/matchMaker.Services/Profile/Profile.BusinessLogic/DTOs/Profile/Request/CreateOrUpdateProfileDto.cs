using Common.Constants;

namespace Profile.BusinessLogic.DTOs.Profile.Request;

public class CreateOrUpdateProfileDto
{
    public string Name { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public int? Height { get; set; }
    public bool ShowAge { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public int MaxDistance { get; set; }
    public Gender PreferredGender { get; set; } = Gender.Undefined;
    public long? GoalId { get; set; }
    public long CityId { get; set; }
    public long UserId { get; set; }
    public ICollection<long> LanguageIds { get; set; } = new List<long>();
    public ICollection<long> EducationIds { get; set; } = new List<long>();
    public ICollection<long> InterestIds { get; set; } = new List<long>();
}