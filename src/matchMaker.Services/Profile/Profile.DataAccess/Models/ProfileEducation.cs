namespace Profile.DataAccess.Models;

public class ProfileEducation
{
    public long ProfileId { get; set; }
    public UserProfile Profile { get; set; }
    
    public long EducationId { get; set; }
    public Education Education { get; set; }
    
    public string Description { get; set; }
}