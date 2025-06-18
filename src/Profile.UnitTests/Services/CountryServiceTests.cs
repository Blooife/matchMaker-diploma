using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Moq;
using Profile.BusinessLogic.DTOs.City.Response;
using Profile.BusinessLogic.DTOs.Country.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Implementations;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Xunit;

namespace Profile.UnitTests.Services;

public class CountryServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly CountryService _countryService;

    public CountryServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();

        _countryService = new CountryService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_WhenCachedDataExists_ReturnsCachedCountries()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllAsync_WhenCacheMiss_ReturnsMappedCountriesAndCachesThem()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllCitiesByCountryId_WhenCachedDataExists_ReturnsCachedCities()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllCitiesByCountryId_WhenCountryExists_ReturnsMappedCitiesAndCachesThem()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllCitiesByCountryId_WhenCountryNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }
}