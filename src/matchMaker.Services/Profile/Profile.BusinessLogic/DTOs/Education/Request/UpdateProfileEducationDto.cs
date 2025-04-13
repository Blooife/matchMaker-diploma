namespace Profile.BusinessLogic.DTOs.Education.Request;

public class UpdateProfileEducationDto
{
    public string ProfileId { get; set; }
    public int EducationId { get; set; }
    public string Description { get; set; }
}