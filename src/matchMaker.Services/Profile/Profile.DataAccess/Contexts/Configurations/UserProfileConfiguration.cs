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
            .UsingEntity<Dictionary<string, object>>(
                "ProfileInterest",
                r => r.HasOne<Interest>().WithMany().HasForeignKey("InterestId")
                    .HasConstraintName("FK_ProfileInterest_Interest_InterestId"),
                l => l.HasOne<UserProfile>().WithMany().HasForeignKey("ProfileId")
                    .HasConstraintName("FK_ProfileInterest_UserProfile_ProfileId"));
        
        builder
            .HasMany(userProfile => userProfile.Languages)
            .WithMany(language => language.Profiles)
            .UsingEntity<Dictionary<string, object>>(
                "ProfileLanguage",
                r => r.HasOne<Language>().WithMany().HasForeignKey("LanguageId")
                    .HasConstraintName("FK_ProfileLanguage_Language_InterestId"),
                l => l.HasOne<UserProfile>().WithMany().HasForeignKey("ProfileId")
                    .HasConstraintName("FK_ProfileLanguage_UserProfile_ProfileId"));
        
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