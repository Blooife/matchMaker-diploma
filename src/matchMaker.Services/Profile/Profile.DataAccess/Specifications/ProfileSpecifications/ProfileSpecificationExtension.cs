using Profile.DataAccess.Models;

namespace Profile.DataAccess.Specifications.ProfileSpecifications;

public static class ProfileSpecificationExtension
{
    public static bool ContainsInterest(this UserProfile profile, long interestId)
    {
        var spec = new ContainsInterestSpecification(interestId);
        bool result = spec.IsSatisfied(profile);
        return result;
    }
    
    public static bool InterestsLessThan(this UserProfile profile, long lessThanCount)
    {
        var spec = new InterestsLessThanSpecification(lessThanCount);
        bool result = spec.IsSatisfied(profile);
        return result;
    }
    
    public static bool ContainsLanguage(this UserProfile profile, long languageId)
    {
        var spec = new ContainsLanguageSpecification(languageId);
        bool result = spec.IsSatisfied(profile);
        return result;
    }
    
    public static bool ContainsEducation(this UserProfile profile, long educationId)
    {
        var spec = new ContainsEducationSpecification(educationId);
        bool result = spec.IsSatisfied(profile);
        return result;
    }
}