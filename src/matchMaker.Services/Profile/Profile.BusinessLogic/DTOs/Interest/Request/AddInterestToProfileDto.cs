namespace Profile.BusinessLogic.DTOs.Interest.Request;

public class AddInterestToProfileDto
{
    public long ProfileId { get; set; }
    public long InterestId { get; set; }
}