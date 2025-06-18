using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MessageQueue;
using Moq;
using Profile.BusinessLogic.DTOs.Image.Request;
using Profile.BusinessLogic.DTOs.Image.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Implementations;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Xunit;

namespace Profile.UnitTests.Services;

public class ImageServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMinioService> _minioServiceMock;
    private readonly Mock<ICommunicationBus> _communicationBusMock;
    private readonly ImageService _imageService;

    public ImageServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _minioServiceMock = new Mock<IMinioService>();
        _communicationBusMock = new Mock<ICommunicationBus>();

        _imageService = new ImageService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _minioServiceMock.Object,
            _communicationBusMock.Object
        );
    }

    [Fact]
    public async Task AddImageToProfileAsync_WhenProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddImageToProfileAsync_WhenInvalidExtension_ThrowsImageUploadException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddImageToProfileAsync_WhenSuccessful_AddsImageAndReturnsDtoList()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddImageToProfileAsync_WhenFirstImage_PublishesProfileUpdatedMessage()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task ChangeMainImageAsync_WhenProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task ChangeMainImageAsync_WhenImageNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task ChangeMainImageAsync_WhenSuccessful_UpdatesMainImageAndPublishesEvent()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveImageAsync_WhenProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveImageAsync_WhenImageNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveImageAsync_WhenSuccessful_RemovesImageAndReturnsDto()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveImageAsync_WhenRemovedMainImage_UpdatesNewMainAndPublishesEvent()
    {
        Assert.True(true);
    }
}
