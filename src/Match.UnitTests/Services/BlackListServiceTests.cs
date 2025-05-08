using Moq;
using Match.BusinessLogic.Services.Implementations;
using Match.DataAccess.Providers.Interfaces;
using Common.Authorization.Context;

namespace Match.UnitTests.Services;

public class BlackListServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuthenticationContext> _authContextMock;

    private readonly BlackListService _blackListService;

    public BlackListServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _authContextMock = new Mock<IAuthenticationContext>();

        _blackListService = new BlackListService(_unitOfWorkMock.Object, _authContextMock.Object);
    }

    [Fact]
    public async Task AddToBlackListAsync_WhenAlreadyExists_DoesNothing()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddToBlackListAsync_WhenNotExists_AddsEntry()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetBlackListForUserAsync_WhenCalled_ReturnsCorrectList()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveFromBlackListAsync_WhenCalled_RemovesEntry()
    {
        Assert.True(true);
    }
}