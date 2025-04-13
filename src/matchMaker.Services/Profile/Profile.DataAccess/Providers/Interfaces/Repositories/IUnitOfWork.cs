namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserProfileRepository ProfileRepository { get; }
    ILanguageRepository LanguageRepository { get; }
    IGoalRepository GoalRepository { get; }
    ICountryRepository CountryRepository { get; }
    ICityRepository CityRepository { get; }
    IInterestRepository InterestRepository { get; }
    IEducationRepository EducationRepository { get; }
    IImageRepository ImageRepository { get; }
    IUserRepository UserRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}