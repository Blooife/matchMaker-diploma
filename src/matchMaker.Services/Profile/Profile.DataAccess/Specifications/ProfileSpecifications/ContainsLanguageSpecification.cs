using Profile.DataAccess.Models;

namespace Profile.DataAccess.Specifications.ProfileSpecifications;

public class ContainsLanguageSpecification : ExpressionSpecification<UserProfile>
{
    public ContainsLanguageSpecification(long languageId) : base(p => p.Languages.Any(i => i.Id == languageId))
    {
    }
}