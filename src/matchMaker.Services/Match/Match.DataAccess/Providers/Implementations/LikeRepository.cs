using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Implementations;

public class LikeRepository(IMongoCollection<Like> _collection) : GenericRepository<Like, string>(_collection), ILikeRepository
{
    public async Task<Like?> CheckMutualLikeAsync(Like likeParam, CancellationToken cancellationToken)
    {
        var getResult = await GetAsync(like =>
            like.IsLike && like.ProfileId == likeParam.TargetProfileId && like.TargetProfileId == likeParam.ProfileId, cancellationToken);
        
        return getResult.FirstOrDefault();
    }
}