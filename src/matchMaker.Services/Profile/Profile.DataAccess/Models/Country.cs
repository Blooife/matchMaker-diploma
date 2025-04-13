namespace Profile.DataAccess.Models;

public class Country : BaseModel<long>
{
    public string Name { get; set; }

    public List<City> Cities { get; set; } = new ();
}