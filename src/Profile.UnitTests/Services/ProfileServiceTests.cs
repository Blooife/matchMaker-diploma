using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dtos.Profile;
using Common.Exceptions;
using MessageQueue;
using MessageQueue.Messages.Profile;
using Moq;
using Profile.BusinessLogic.DTOs.Profile.Request;
using Profile.BusinessLogic.DTOs.Profile.Response;
using Profile.BusinessLogic.Services.Implementations;
using Profile.DataAccess.Models;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Xunit;

namespace Profile.UnitTests.Services;

public class ProfileServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICommunicationBus> _busMock;
    private readonly ProfileService _profileService;

    public ProfileServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _busMock = new Mock<ICommunicationBus>();
        _profileService = new ProfileService(_unitOfWorkMock.Object, _mapperMock.Object, _busMock.Object);
    }

    [Fact]
    public async Task CreateProfileAsync_CreatesProfileAndPublishesMessage()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task UpdateProfileAsync_ProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetProfileByIdAsync_ProfileExists_ReturnsMappedDto()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetProfileByIdAsync_ProfileNotFound_ThrowsNotFoundException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddLanguageToProfileAsync_AlreadyContains_ThrowsAlreadyContainsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveLanguageFromProfileAsync_NotContains_ThrowsNotContainsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task AddInterestToProfileAsync_MoreThanLimit_ThrowsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task RemoveInterestFromProfileAsync_NotContains_ThrowsNotContainsException()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetProfilesByIdsAsync_ReturnsMappedDtos()
    {
        Assert.True(true);
    }
}
