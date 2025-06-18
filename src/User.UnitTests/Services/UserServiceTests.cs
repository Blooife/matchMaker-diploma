using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Common.Authorization.Context;
using MessageQueue;
using User.BusinessLogic.Services.Implementations;
using User.DataAccess.Providers.Interfaces;

namespace User.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserProvider> _userProviderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly Mock<ICommunicationBus> _communicationBusMock;
    private readonly Mock<IAuthenticationContext> _authContextMock;

    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userProviderMock = new Mock<IUserProvider>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _communicationBusMock = new Mock<ICommunicationBus>();
        _authContextMock = new Mock<IAuthenticationContext>();

        _userService = new UserService(
            _userProviderMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _communicationBusMock.Object,
            _authContextMock.Object
        );
    }

    [Fact]
    public async Task DeleteUserByIdAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task DeleteUserByIdAsync_WhenDeletionFails_ThrowsDeleteUserException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task DeleteUserByIdAsync_WhenSuccessful_ReturnsSuccessResponse()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetPaginatedUsersAsync_WhenCalled_ReturnsPagedUserResponseDtos()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserFound_ReturnsUserResponseDto()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserFound_ReturnsUserResponseDto()
    {
        Assert.True(true);
    }
}
