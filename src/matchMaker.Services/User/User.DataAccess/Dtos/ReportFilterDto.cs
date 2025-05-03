using Common.Enums;

namespace User.DataAccess.Dtos;

public class ReportFilterDto
{
    public ReportStatus? Status { get; set; }
    public long? ReporterUserId { get; set; }
    public long? ReportedUserId { get; set; }
    public long? ReportTypeId { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}