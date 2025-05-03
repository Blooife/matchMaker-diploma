using Common.Authorization.Context;
using Match.BusinessLogic.DTOs.BlackList;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.BusinessLogic.Services.Implementations;

public class BlackListService(IUnitOfWork unitOfWork, IAuthenticationContext _authenticationContext) : IBlackListService
{
    public async Task AddToBlackListAsync(CreateBlackListDto dto, CancellationToken cancellationToken = default)
    {
        var exists = await unitOfWork.BlackLists.ExistsAsync(_authenticationContext.UserId, dto.BlockedProfileId, cancellationToken);

        if (exists)
        {
            return;
        }

        var entry = new BlackList
        {
            BlockerProfileId = _authenticationContext.UserId,
            BlockedProfileId = dto.BlockedProfileId,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.BlackLists.CreateAsync(entry, cancellationToken);
    }

    public async Task<List<BlackListResponseDto>> GetBlackListForUserAsync(CancellationToken cancellationToken = default)
    {
        var entries = await unitOfWork.BlackLists.GetByBlockerIdAsync(_authenticationContext.UserId, cancellationToken);
        var profileIds = entries.Select(e => e.BlockedProfileId).ToList();
        var profiles = await unitOfWork.Profiles.GetAsync(p => profileIds.Contains(p.Id), cancellationToken);
        var profileDict = profiles.ToDictionary(p => p.Id);

        return entries.Select(e =>
        {
            profileDict.TryGetValue(e.BlockedProfileId, out var profile);

            return new BlackListResponseDto
            {
                Id = e.Id,
                BlockerProfileId = e.BlockerProfileId,
                BlockedProfileId = e.BlockedProfileId,
                CreatedAt = e.CreatedAt,
                BlockedProfileFullName = $"{profile?.LastName} {profile?.Name}",
                BlockedProfileMainImageUrl = profile?.MainImageUrl,
            };
        }).ToList();
    }

    public async Task RemoveFromBlackListAsync(RemoveFromBlackListDto dto, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BlackLists.RemoveAsync(_authenticationContext.UserId, dto.BlockedProfileId, cancellationToken);
    }
}