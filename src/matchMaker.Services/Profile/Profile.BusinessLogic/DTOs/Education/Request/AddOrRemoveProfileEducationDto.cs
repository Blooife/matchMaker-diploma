namespace Profile.BusinessLogic.DTOs.Education.Request;

public class AddOrRemoveProfileEducationDto
{
    public long ProfileId { get; set; }
    public long EducationId { get; set; }
    public string Description { get; set; }
}