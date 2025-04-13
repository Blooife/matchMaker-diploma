using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Contexts.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasIndex(education => education.Name).IsUnique();
        builder
            .HasMany(education => education.ProfileEducations)
            .WithOne(userEducation => userEducation.Education)
            .HasForeignKey(userEducation => userEducation.EducationId);
    }
}