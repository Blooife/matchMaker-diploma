namespace Profile.DataAccess.Models;

public class City : BaseModel<long>
{
    public string Name { get; set; }
    
    public long CountryId { get; set; }
    public Country Country { get; set; }

    public List<UserProfile> Profiles { get; set; } = new ();
}