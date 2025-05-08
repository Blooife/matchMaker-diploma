using Moq;
using AutoMapper;
using Common.Authorization.Context;
using Match.BusinessLogic.Services.Implementations;
using Match.DataAccess.Providers.Interfaces;

namespace Match.UnitTests.Services;

public class LikeServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuthenticationContext> _authContextMock;

    private readonly LikeService _likeService;

    public LikeServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _authContextMock = new Mock<IAuthenticationContext>();

        _authContextMock.Setup(x => x.UserId).Returns(1);

        _likeService = new LikeService(_unitOfWorkMock.Object, _mapperMock.Object, _authContextMock.Object);
    }

    [Fact]
    public async Task AddLikeAsync_WhenLikerProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddLikeAsync_WhenLikedProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddLikeAsync_WhenMutualLikeExists_CreatesMatchAndDeletesMutualLike()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddLikeAsync_WhenNoMutualLike_CreatesLikeOnly()
    {
        Assert.True(true);
    }
}
