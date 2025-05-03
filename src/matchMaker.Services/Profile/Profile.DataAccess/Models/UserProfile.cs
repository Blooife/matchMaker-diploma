using Common.Constants;
using Common.Interfaces;

namespace Profile.DataAccess.Models;

public class UserProfile : BaseModel<long>, ISoftDeletable
{
    public string Name { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public int? Height { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public int MaxDistance { get; set; }
    public Gender PreferredGender { get; set; } = Gender.Undefined;
    public long UserId { get; set; }
    public long? GoalId { get; set; }
    public long CityId { get; set; }
    
    public City City { get; set; }
    public User User { get; set; }
    public Goal? Goal { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<Language> Languages { get; set; } = new ();
    public List<Interest> Interests { get; set; } = new ();
    public List<ProfileEducation> ProfileEducations { get; set; } = new ();
    public List<Image> Images { get; set; } = new ();
}