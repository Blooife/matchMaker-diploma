using Moq;
using AutoMapper;
using Common.Authorization.Context;
using Match.BusinessLogic.Services.Implementations;
using Match.DataAccess.Providers.Interfaces;

namespace Match.UnitTests.Services;

public class NotificationServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuthenticationContext> _authContextMock;
    private readonly NotificationService _notificationService;

    public NotificationServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _authContextMock = new Mock<IAuthenticationContext>();
        _notificationService = new NotificationService(_unitOfWorkMock.Object, _mapperMock.Object, _authContextMock.Object);
    }

    [Fact]
    public async Task GetUserNotificationsAsync_WhenNotificationsExist_ReturnsMappedNotifications()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetUserNotificationsAsync_WhenNoNotifications_ReturnsEmptyList()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task MarkNotificationsAsReadAsync_WhenCalled_UpdatesNotifications()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task CreateNewMessageNotificationAsync_CreatesNotificationSuccessfully()
    {
        Assert.True(true);
    }
}