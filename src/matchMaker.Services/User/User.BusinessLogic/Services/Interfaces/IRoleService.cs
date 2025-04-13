using User.BusinessLogic.DTOs.Response;
using Common.Models;

namespace User.BusinessLogic.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken);
    Task<GeneralResponseDto> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken);
    Task<GeneralResponseDto> RemoveUserFromRoleAsync(string userId, string roleName,
        CancellationToken cancellationToken);
}