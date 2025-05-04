using Common.Enums;

namespace User.DataAccess.Dtos;

public class ReportFilterDto
{
    public ReportStatus? Status { get; set; }
    public string? ReporterUserEmail { get; set; }
    public string? ReportedUserEmail { get; set; }
    public long? ReportTypeId { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public bool? NotReviewed { get; set; }
}