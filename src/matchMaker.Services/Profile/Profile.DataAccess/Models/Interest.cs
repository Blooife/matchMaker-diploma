namespace Profile.DataAccess.Models;

public class Interest : BaseModel<long>
{
    public string Name { get; set; }

    public List<UserProfile> Profiles { get; set; } = new ();
}