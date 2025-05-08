using User.BusinessLogic.DTOs.Response;
using AutoMapper;
using Common.Authorization.Context;
using Common.Exceptions;
using Common.Models;
using MessageQueue;
using MessageQueue.Messages.User;
using Microsoft.Extensions.Logging;
using User.BusinessLogic.Services.Interfaces;
using User.DataAccess.Providers.Interfaces;

namespace User.BusinessLogic.Services.Implementations;

public class UserService(
    IUserProvider _userRepository,
    IMapper _mapper,
    ILogger<UserService> _logger,
    ICommunicationBus _communicationBus,
    IAuthenticationContext _authenticationContext) : IUserService

{
    public async Task<GeneralResponseDto> DeleteUserByIdAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError("Delete user by id failed: User with id = {userId} was not found", userId);
            throw new NotFoundException("Пользователь");
        }
        
        var result = await _userRepository.DeleteUserByIdAsync(user);
        
        if (!result.Succeeded)
        {
            _logger.LogError("Delete user by id failed with errors: {errors}", result.Errors.Select(e => e.Description).ToArray());
            throw new DeleteUserException(ExceptionMessages.DeleteUserFailed);
        }

        await _communicationBus.PublishAsync(new UserDeletedEventMessage()
        {
            Id = user.Id,
        });
        
        return new GeneralResponseDto() { Message = "User deleted successfully" };
    }
    
    public async Task<PagedList<UserResponseDto>> GetPaginatedUsersAsync(int pageSize, int pageNumber)
    {
        var (users, totalCount) = await _userRepository.GetPagedUsersAsync(pageNumber, pageSize);

        users = users.Where(x => x.Id != _authenticationContext.UserId).ToList();
        var userResponseDtos = _mapper.Map<List<UserResponseDto>>(users);

        for (int i = 0; i < userResponseDtos.Count; i++)
        {
            userResponseDtos[i].Roles = await _userRepository.GetRolesAsync(users[i]);
        }
        
        return new PagedList<UserResponseDto>(userResponseDtos, totalCount, pageNumber, pageSize);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Get user by id failed: User with id = {userId} was not found", userId);
            throw new NotFoundException("Пользователь");
        }
        
        return _mapper.Map<UserResponseDto>(user);
    }
    
    public async Task<UserResponseDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError("Get user by email failed: User with email = {email} was not found", email);
            throw new NotFoundException("Пользователь");
        }
        
        return _mapper.Map<UserResponseDto>(user);
    }
}