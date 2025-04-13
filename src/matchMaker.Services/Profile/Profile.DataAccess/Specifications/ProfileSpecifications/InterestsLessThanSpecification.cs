using Profile.DataAccess.Models;

namespace Profile.DataAccess.Specifications.ProfileSpecifications;

public class InterestsLessThanSpecification : ExpressionSpecification<UserProfile>
{
    public InterestsLessThanSpecification(long lessThanCount) : base(p=> p.Interests.Count < lessThanCount)
    {
    }
}