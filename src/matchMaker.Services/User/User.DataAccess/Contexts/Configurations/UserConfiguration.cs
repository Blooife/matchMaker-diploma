using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.DataAccess.Contexts.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<Models.User>
{
    public void Configure(EntityTypeBuilder<Models.User> builder)
    {
        builder.Ignore(user => user.PhoneNumber);
        builder.Ignore(user => user.PhoneNumberConfirmed);
        builder.Ignore(user => user.TwoFactorEnabled);
        builder.Ignore(user => user.LockoutEnd);
        builder.Ignore(user => user.LockoutEnabled);
        builder.Ignore(user => user.PhoneNumber);
        builder.Property(user => user.Email).IsRequired();
        builder.Property(user => user.PasswordHash).IsRequired();
        builder.Property(user => user.IsBanned).HasDefaultValue(false);
        builder.Property(user => user.BannedUntil).HasDefaultValue(null);
    }
}