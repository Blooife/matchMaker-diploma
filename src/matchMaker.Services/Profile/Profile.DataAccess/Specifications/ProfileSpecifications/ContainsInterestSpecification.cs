using Profile.DataAccess.Models;

namespace Profile.DataAccess.Specifications.ProfileSpecifications;

public class ContainsInterestSpecification : ExpressionSpecification<UserProfile>
{
    public ContainsInterestSpecification(long interestId) : base(p => p.Interests.Any(i => i.Id == interestId))
    {
    }
}