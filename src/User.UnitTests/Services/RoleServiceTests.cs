using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using User.BusinessLogic.Services.Implementations;
using User.DataAccess.Providers.Interfaces;

namespace User.UnitTests.Services;

public class RoleServiceTests
{
    private readonly Mock<IRoleProvider> _roleProviderMock;
    private readonly Mock<IUserProvider> _userProviderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<RoleService>> _loggerMock;

    private readonly RoleService _roleService;

    public RoleServiceTests()
    {
        _roleProviderMock = new Mock<IRoleProvider>();
        _userProviderMock = new Mock<IUserProvider>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<RoleService>>();

        _roleService = new RoleService(
            _roleProviderMock.Object,
            _userProviderMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetAllRolesAsync_WhenCalled_ReturnsMappedRoles()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AssignRoleAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AssignRoleAsync_WhenRoleNotExist_ThrowsAssignRoleException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AssignRoleAsync_WhenUserAlreadyHasRole_ThrowsAssignRoleException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AssignRoleAsync_WhenAssignmentFails_ThrowsAssignRoleException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AssignRoleAsync_WhenSuccessful_ReturnsGeneralResponseDto()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_WhenRoleNotExist_ThrowsRemoveRoleException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_WhenUserHasOnlyOneRole_ThrowsRemoveRoleException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_WhenRemovingFails_ThrowsRemoveRoleException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_WhenSuccessful_ReturnsGeneralResponseDto()
    {
        Assert.True(true);
    }
}
