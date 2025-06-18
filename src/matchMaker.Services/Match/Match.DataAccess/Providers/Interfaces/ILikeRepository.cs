using Match.DataAccess.Dtos;
using Match.DataAccess.Models;

namespace Match.DataAccess.Providers.Interfaces;

public interface ILikeRepository : IGenericRepository<Like, string>
{
    Task<Like?> CheckMutualLikeAsync(Like likeParam, CancellationToken cancellationToken);
    Task<List<LikeCountDto>> GetLikesCountAsync(IEnumerable<long> profileIds);
}