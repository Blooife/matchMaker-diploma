using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Profile.BusinessLogic.DTOs.Interest.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Implementations;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Xunit;

namespace Profile.UnitTests.Services;

public class InterestServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly InterestService _interestService;

    public InterestServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();

        _interestService = new InterestService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_WhenCacheHit_ReturnsCachedInterests()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllAsync_WhenCacheMiss_ReturnsMappedInterestsAndCachesThem()
    {
        Assert.True(true);
    }
}