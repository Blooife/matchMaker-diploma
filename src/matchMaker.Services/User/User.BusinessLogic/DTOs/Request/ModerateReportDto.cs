using Common.Enums;

namespace User.BusinessLogic.DTOs.Request;

public class ModerateReportDto
{
    public long ReportId { get; set; }
    public ReportStatus Status { get; set; }
    public string? ModeratorComment { get; set; }
    public DateTime? BanUntil { get; set; }
}