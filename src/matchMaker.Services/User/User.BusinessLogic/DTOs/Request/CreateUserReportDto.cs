namespace User.BusinessLogic.DTOs.Request;

public class CreateUserReportDto
{
    public long ReportedUserId { get; set; }
    public long ReportTypeId { get; set; }
    public string Reason { get; set; } = string.Empty;
}