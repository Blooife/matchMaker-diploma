using Profile.DataAccess.Models;

namespace Profile.DataAccess.Specifications.ProfileSpecifications;

public class ContainsEducationSpecification : ExpressionSpecification<UserProfile>
{
    public ContainsEducationSpecification(long educationId) : base(p => p.ProfileEducations.Any(i => i.EducationId == educationId))
    {
    }
}