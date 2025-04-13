using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Contexts.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasIndex(i => new { i.IsMainImage, i.UploadTimestamp })
            .HasDatabaseName("IX_IsMainImage_UploadTimestamp")
            .IsDescending(true, true);
    }
}