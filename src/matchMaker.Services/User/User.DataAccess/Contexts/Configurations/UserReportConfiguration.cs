using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.DataAccess.Models;

namespace User.DataAccess.Contexts.Configurations;

public class UserReportConfiguration : IEntityTypeConfiguration<UserReport>
{
    
    public void Configure(EntityTypeBuilder<UserReport> builder)
    {
        builder
            .HasIndex(r => new { r.ReporterUserId, r.ReportedUserId })
            .IsUnique();

        builder
            .HasOne(r => r.Reporter)
            .WithMany(u => u.ReportsMade)
            .HasForeignKey(r => r.ReporterUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.Reported)
            .WithMany(u => u.ReportsReceived)
            .HasForeignKey(r => r.ReportedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(r => r.Moderator)
            .WithMany(u => u.ReportsReviewed)
            .HasForeignKey(r => r.ReviewedByModeratorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}