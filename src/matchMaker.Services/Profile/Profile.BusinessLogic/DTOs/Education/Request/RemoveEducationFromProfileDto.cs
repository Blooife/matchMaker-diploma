namespace Profile.BusinessLogic.DTOs.Education.Request;

public class RemoveEducationFromProfileDto
{
    public string ProfileId { get; set; }
    public int EducationId { get; set; }
}