namespace Profile.DataAccess.Models;

public class Language : BaseModel<long>
{
    public string Name { get; set; }

    public List<UserProfile> Profiles { get; set; } = new ();
}