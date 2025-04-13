using Profile.DataAccess.Contexts;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfileDbContext _dbContext;
    private IUserProfileRepository _profileRepository;
    private ILanguageRepository _languageRepository;
    private IGoalRepository _goalRepository;
    private ICountryRepository _countryRepository;
    private ICityRepository _cityRepository;
    private IInterestRepository _interestRepository;
    private IEducationRepository _educationRepository;
    private IImageRepository _imageRepository;
    private IUserRepository _userRepository;
    
    
    public UnitOfWork(ProfileDbContext context)
    {
        _dbContext = context;
    }

    public IUserProfileRepository ProfileRepository => _profileRepository ??= new UserProfileRepository(_dbContext);
    public ILanguageRepository LanguageRepository => _languageRepository ??= new LanguageRepository(_dbContext);
    public IGoalRepository GoalRepository => _goalRepository ??= new GoalRepository(_dbContext);
    public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_dbContext);
    public ICityRepository CityRepository => _cityRepository ??= new CityRepository(_dbContext);
    public IInterestRepository InterestRepository => _interestRepository ??= new InterestRepository(_dbContext);
    public IEducationRepository EducationRepository => _educationRepository ??= new EducationRepository(_dbContext);
    public IImageRepository ImageRepository => _imageRepository ??= new ImageRepository(_dbContext);
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_dbContext);
    
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    public Task SaveAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}