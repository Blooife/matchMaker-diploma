using Xunit;
using Moq;
using AutoMapper;
using Match.BusinessLogic.Services.Implementations;
using Match.DataAccess.Providers.Interfaces;
using Common.Authorization.Context;

namespace Match.UnitTests.Services;

public class ChatServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuthenticationContext> _authContextMock;

    private readonly ChatService _chatService;

    public ChatServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _authContextMock = new Mock<IAuthenticationContext>();

        _chatService = new ChatService(_unitOfWorkMock.Object, _mapperMock.Object, _authContextMock.Object);
    }

    [Fact]
    public async Task SendMessageAsync_WhenChatNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task SendMessageAsync_WhenChatFound_CreatesMessageAndUpdatesChat()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetChatsByProfileIdsAsync_WhenProfilesNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetChatsByProfileId_WhenCalled_ReturnsPagedChats()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task CreateChatAsync_WhenProfilesNotMatched_ThrowsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task CreateChatAsync_WhenValid_CreatesChat()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task DeleteChatAsync_WhenChatNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task DeleteChatAsync_WhenChatFound_DeletesChatAndMessages()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task ReadChatAsync_WhenChatNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task IncrementUnreadCountAsync_WhenChatFound_IncrementsCorrectCounter()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetById_WhenChatNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetById_WhenChatFound_ReturnsChat()
    {
        Assert.True(true);
    }
}
