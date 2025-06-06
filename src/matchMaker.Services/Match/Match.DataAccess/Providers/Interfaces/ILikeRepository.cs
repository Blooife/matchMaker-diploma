using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.DataAccess.Providers.Interfaces;

public interface ILikeRepository : IGenericRepository<Like, string>
{
    Task<Like?> CheckMutualLikeAsync(Like likeParam, CancellationToken cancellationToken);
}