using User.BusinessLogic.DTOs.Response;
using User.BusinessLogic.Exceptions;
using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Microsoft.Extensions.Logging;
using User.BusinessLogic.Services.Interfaces;
using User.DataAccess.Providers.Interfaces;

namespace User.BusinessLogic.Services.Implementations;

public class RoleService(IRoleProvider _roleRepository, IUserProvider _userRepository, IMapper _mapper, ILogger<RoleService> _logger) : IRoleService
{
    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles =  await _roleRepository.GetAllRolesAsync(cancellationToken); 
        
        return _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
    }
    
    public async Task<GeneralResponseDto> AssignRoleAsync(string email, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError("Assign role failed: User with email = {email} was not found", email);
            throw new NotFoundException(email);
        }

        var isRoleExist = await _roleRepository.RoleExistsAsync(roleName);
        
        if (!isRoleExist)
        {
            _logger.LogError("Assign role failed: role with name = {name} does not exist", roleName);
            throw new AssignRoleException(ExceptionMessages.RoleNotExists);
        }
        
        var result = await _userRepository.AddToRoleAsync(user, roleName);
        
        if (!result.Succeeded)
        {
            _logger.LogError("Assign role failed with errors: {errors}", result.Errors.Select(e => e.Description).ToArray());
            throw new AssignRoleException(result.Errors.First().Description);
        }
        
        return new GeneralResponseDto { Message = "Role assigned successfully" };
    }
    
    public async Task<GeneralResponseDto> RemoveUserFromRoleAsync(string email, string roleName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        
        if (user is null)
        {
            _logger.LogError("Remove user from role failed: User with email = {email} was not found", email);
            throw new NotFoundException(email);
        }
        
        var isRoleExist = await _roleRepository.RoleExistsAsync(roleName);
        
        if (!isRoleExist)
        {
            _logger.LogError("Assign role failed: role with name = {name} does not exist", roleName);
            throw new RemoveRoleException(ExceptionMessages.RoleNotExists);
        }

        var roles = await _userRepository.GetRolesAsync(user);
        
        if (roles.Count == 1)
        {
            _logger.LogError("Remove user from role failed: User can't have less than 1 role");
            throw new RemoveRoleException("User can't have less than 1 role");
        }
        
        var result = await _userRepository.RemoveFromRoleAsync(user, roleName);
        
        if (!result.Succeeded)
        {
            _logger.LogError("Remove user from role failed with errors: {errors}", result.Errors.Select(e => e.Description).ToArray());
            throw new RemoveRoleException(result.Errors.First().Description);
        }
        
        return new GeneralResponseDto { Message = "Role removed successfully" };
    }
}