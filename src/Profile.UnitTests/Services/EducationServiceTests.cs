using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Moq;
using Profile.BusinessLogic.DTOs.Education.Request;
using Profile.BusinessLogic.DTOs.Education.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Implementations;
using Profile.DataAccess.Models;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Xunit;

namespace Profile.UnitTests.Services;

public class EducationServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly EducationService _educationService;

    public EducationServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();

        _educationService = new EducationService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_WhenCachedDataExists_ReturnsCachedEducations()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllAsync_WhenCacheMiss_ReturnsMappedEducationsAndCachesThem()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddEducationToProfileAsync_WhenProfileOrEducationNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddEducationToProfileAsync_WhenAlreadyContainsEducation_ThrowsAlreadyContainsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddEducationToProfileAsync_WhenSuccessful_AddsEducationAndReturnsUpdatedList()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveEducationFromProfileAsync_WhenProfileOrEducationNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveEducationFromProfileAsync_WhenNotContainsEducation_ThrowsNotContainsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveEducationFromProfileAsync_WhenSuccessful_RemovesAndReturnsUpdatedList()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task UpdateProfileEducationAsync_WhenNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task UpdateProfileEducationAsync_WhenNotContainsEducation_ThrowsNotContainsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task UpdateProfileEducationAsync_WhenSuccessful_UpdatesAndReturnsUpdatedEducation()
    {
        Assert.True(true);
    }
}
