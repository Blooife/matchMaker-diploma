using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Contexts.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(userProfile => userProfile.Id).ValueGeneratedOnAdd();
        
        builder
            .HasMany(userProfile => userProfile.Interests)
            .WithMany(interest => interest.Profiles)
            .UsingEntity("ProfileInterest");
        
        builder
            .HasMany(userProfile => userProfile.Languages)
            .WithMany(language => language.Profiles)
            .UsingEntity("ProfileLanguage");
        
        builder
            .HasMany(up => up.ProfileEducations)
            .WithOne(upi => upi.Profile)
            .HasForeignKey(upi => upi.ProfileId);
        
        builder
            .HasMany(userProfile => userProfile.Images)
            .WithOne(i => i.Profile)
            .HasForeignKey(i => i.ProfileId);
    }
}