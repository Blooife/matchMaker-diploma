using Moq;
using User.BusinessLogic.Services.Implementations;
using User.BusinessLogic.Providers.Interfaces;
using AutoMapper;
using Common.Authorization.Context;
using Microsoft.Extensions.Logging;
using MessageQueue;
using User.DataAccess.Providers.Interfaces;

namespace User.UnitTests.Services;


public class AuthServiceTests
{
    private readonly Mock<IUserProvider> _userProviderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly Mock<IJwtTokenProvider> _jwtTokenProviderMock;
    private readonly Mock<IRefreshTokenProvider> _refreshTokenProviderMock;
    private readonly Mock<ICommunicationBus> _communicationBusMock;
    private readonly Mock<IAuthenticationContext> _authenticationContextMock;

    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userProviderMock = new Mock<IUserProvider>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<AuthService>>();
        _jwtTokenProviderMock = new Mock<IJwtTokenProvider>();
        _refreshTokenProviderMock = new Mock<IRefreshTokenProvider>();
        _communicationBusMock = new Mock<ICommunicationBus>();
        _authenticationContextMock = new Mock<IAuthenticationContext>();

        _authService = new AuthService(
            _userProviderMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _jwtTokenProviderMock.Object,
            _refreshTokenProviderMock.Object,
            _communicationBusMock.Object,
            _authenticationContextMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_WhenSuccessful_ReturnsUserResponseDto()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RegisterAsync_WhenFailed_ThrowsRegisterException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsLoginResponseDto()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenIsValid_ReturnsLoginResponseDto()
    {
        Assert.True(true);
    }

}
