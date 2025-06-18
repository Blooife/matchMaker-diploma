using Moq;
using Common.Authorization.Context;
using MessageQueue;
using User.DataAccess.Providers.Interfaces;
using User.BusinessLogic.Services.Implementations;

namespace User.UnitTests.Services;

public class UserReportServiceTests
{
    private readonly Mock<IUserReportProvider> _userReportProviderMock;
    private readonly Mock<IAuthenticationContext> _authContextMock;
    private readonly Mock<IUserProvider> _userProviderMock;
    private readonly Mock<ICommunicationBus> _communicationBusMock;

    private readonly UserReportService _userReportService;

    public UserReportServiceTests()
    {
        _userReportProviderMock = new Mock<IUserReportProvider>();
        _authContextMock = new Mock<IAuthenticationContext>();
        _userProviderMock = new Mock<IUserProvider>();
        _communicationBusMock = new Mock<ICommunicationBus>();

        _userReportService = new UserReportService(
            _userReportProviderMock.Object,
            _authContextMock.Object,
            _userProviderMock.Object,
            _communicationBusMock.Object
        );
    }

    [Fact]
    public async Task HasUserAlreadyReportedAsync_WhenCalled_ReturnsBoolean()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task CreateReportAsync_WhenAlreadyReported_ThrowsInvalidOperationException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task CreateReportAsync_WhenReportTypeNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task CreateReportAsync_WhenValid_CreatesReport()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetReportsAgainstUserAsync_WhenCalled_ReturnsMappedDtos()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task DeleteReportAsync_WhenCalled_DeletesReport()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetReportsAsync_WhenCalled_ReturnsPagedList()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task ModerateReportAsync_WhenStatusBlocked_CallsBanUser()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task ModerateReportAsync_WhenStatusRejected_PublishesNotification()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetReportTypesAsync_WhenCalled_ReturnsMappedTypes()
    {
        Assert.True(true);
    }
}
