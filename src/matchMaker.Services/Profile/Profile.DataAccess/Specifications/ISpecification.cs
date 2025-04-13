namespace Profile.DataAccess.Specifications;

public interface ISpecification<in T>
{
    bool IsSatisfied(T obj);
}