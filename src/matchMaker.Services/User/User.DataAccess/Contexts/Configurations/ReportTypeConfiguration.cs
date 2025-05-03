using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.DataAccess.Models;

namespace User.DataAccess.Contexts.Configurations;

public class ReportTypeConfiguration : IEntityTypeConfiguration<ReportType>
{
    public void Configure(EntityTypeBuilder<ReportType> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasMany(rt => rt.Reports)
            .WithOne(r => r.ReportType)
            .HasForeignKey(r => r.ReportTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}