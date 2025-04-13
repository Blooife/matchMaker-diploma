using User.BusinessLogic.DTOs.Response;
using Common.Models;

namespace User.BusinessLogic.Services.Interfaces;

public interface IUserService
{
    Task<GeneralResponseDto> DeleteUserByIdAsync(long userId, CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByIdAsync(long userId, CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<PagedList<UserResponseDto>> GetPaginatedUsersAsync(int pageSize, int pageNumber);
}