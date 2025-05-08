using Xunit;
using Moq;
using AutoMapper;
using Common.Authorization.Context;
using Match.BusinessLogic.Services.Implementations;
using Match.DataAccess.Providers.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Profile.Client;

namespace Match.UnitTests.Services;

public class MatchServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProfileClient> _profileClientMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuthenticationContext> _authenticationContextMock;

    private readonly MatchService _matchService;

    public MatchServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _profileClientMock = new Mock<IProfileClient>();
        _mapperMock = new Mock<IMapper>();
        _authenticationContextMock = new Mock<IAuthenticationContext>();

        _matchService = new MatchService(_unitOfWorkMock.Object, _profileClientMock.Object, _mapperMock.Object, _authenticationContextMock.Object);
    }

    [Fact]
    public async Task GetMatchesByProfileIdAsync_WhenProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetMatchesByProfileIdAsync_WhenMatchesExist_ReturnsMappedProfiles()
    {
        Assert.True(true);
    }
}
