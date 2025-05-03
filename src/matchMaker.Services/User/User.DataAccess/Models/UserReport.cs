using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums;

namespace User.DataAccess.Models;

public class UserReport
{
    [Key]
    public long Id { get; set; }

    [ForeignKey(nameof(Reporter))]
    public long ReporterUserId { get; set; }

    [ForeignKey(nameof(Reported))]
    public long ReportedUserId { get; set; }
    
    [ForeignKey(nameof(ReportType))]
    public long ReportTypeId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = null!;

    public ReportStatus Status { get; set; } = ReportStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ModeratorComment { get; set; }
    
    [ForeignKey(nameof(Moderator))]
    public long? ReviewedByModeratorId { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public ReportType ReportType { get; set; }
    public User Reporter { get; set; }
    public User Reported { get; set; }
    public User? Moderator { get; set; }
}