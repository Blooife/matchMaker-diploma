using Common.Enums;

namespace User.BusinessLogic.DTOs.Response;

public class UserReportDto
{
    public long Id { get; set; }
    public long ReporterUserId { get; set; }
    public long ReportedUserId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public string ReporterEmail { get; set; } = string.Empty;
    public string ReportedEmail { get; set; } = string.Empty;
    
    public string? ModeratorEmail { get; set; }
    
    public string Status { get; set; }
    public long ReportTypeId { get; set; }

    public string ReportType { get; set; } = string.Empty;

    public string? ModeratorComment { get; set; }

    public long? ReviewedByModeratorId { get; set; }

    public DateTime? ReviewedAt { get; set; }
}