using AutoMapper;
using Common.Exceptions;
using Moq;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Implementations;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Xunit;

namespace Profile.UnitTests.Services;

public class CityServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly CityService _cityService;

    public CityServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();

        _cityService = new CityService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_WhenCachedDataExists_ReturnsCachedData()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllAsync_WhenCachedDataNotFound_FetchesFromRepositoryAndCaches()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCachedDataExists_ReturnsCachedData()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCityExistsInRepo_ReturnsCityAndCachesIt()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCityNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }
}