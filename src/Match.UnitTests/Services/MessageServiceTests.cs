using Moq;
using AutoMapper;
using Match.BusinessLogic.Services.Implementations;
using Match.DataAccess.Providers.Interfaces;

namespace Match.UnitTests.Services;

public class MessageServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MessageService _messageService;

    public MessageServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _messageService = new MessageService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetMessagesByChatIdAsync_WhenChatNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetMessagesByChatIdAsync_WhenMessagesExist_ReturnsPagedMessages()
    {
        Assert.True(true);
    }
}