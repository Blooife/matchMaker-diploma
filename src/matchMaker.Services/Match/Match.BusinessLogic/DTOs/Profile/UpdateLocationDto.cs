namespace Match.BusinessLogic.DTOs.Profile;

public class UpdateLocationDto
{
    public long ProfileId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}