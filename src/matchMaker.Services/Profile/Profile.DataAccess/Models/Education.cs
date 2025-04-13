namespace Profile.DataAccess.Models;

public class Education : BaseModel<long>
{
    public string Name { get; set; }
    public List<ProfileEducation> ProfileEducations { get; set; } = new ();
}