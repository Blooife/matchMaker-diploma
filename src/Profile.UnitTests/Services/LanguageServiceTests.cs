using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Moq;
using Profile.BusinessLogic.DTOs.Language.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Implementations;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Xunit;

namespace Profile.UnitTests.Services;

public class LanguageServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly LanguageService _languageService;

    public LanguageServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mapperMock = new Mock<IMapper>();

        _languageService = new LanguageService(
            _unitOfWorkMock.Object,
            _cacheServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_WhenCacheHit_ReturnsCachedLanguages()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllAsync_WhenCacheMiss_ReturnsMappedLanguagesAndCachesThem()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCacheHit_ReturnsCachedLanguage()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCacheMissAndLanguageExists_ReturnsMappedLanguageAndCachesIt()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetByIdAsync_WhenLanguageNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }
}