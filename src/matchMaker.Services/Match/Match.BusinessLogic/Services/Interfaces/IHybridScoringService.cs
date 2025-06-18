using Common.Dtos.Profile;

namespace Match.BusinessLogic.Services.Interfaces;

public interface IHybridScoringService
{
    Task<List<ProfileClientDto>> RankProfilesAsync(
        ProfileClientDto user, List<ProfileClientDto> candidates, CancellationToken cancellationToken);
}