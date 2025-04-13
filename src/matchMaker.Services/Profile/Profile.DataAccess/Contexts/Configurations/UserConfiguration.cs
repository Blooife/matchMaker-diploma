using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Contexts.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Models.User>
{
    public void Configure(EntityTypeBuilder<Models.User> builder)
    {
        builder.HasOne(userProfile => userProfile.Profile).WithOne(user => user.User)
            .HasForeignKey<UserProfile>(userProfile => userProfile.UserId);
    }
}