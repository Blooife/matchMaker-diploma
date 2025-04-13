namespace Profile.BusinessLogic.DTOs.Interest.Request;

public class RemoveInterestFromProfileDto
{
    public string ProfileId { get; set; }
    public long InterestId { get; set; }
}