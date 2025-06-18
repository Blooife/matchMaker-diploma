using Match.DataAccess.Dtos;
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
    
    public async Task<List<LikeCountDto>> GetLikesCountAsync(IEnumerable<long> profileIds)
    {
        var filter = Builders<Like>.Filter.And(
            Builders<Like>.Filter.In(l => l.TargetProfileId, profileIds),
            Builders<Like>.Filter.Eq(l => l.IsLike, true)
        );
        
        var aggregation = _collection.Aggregate()
            .Match(filter)
            .Group(l => l.ProfileId, g => new { ProfileId = g.Key, Count = g.Count() })
            .Project(p => new LikeCountDto { ProfileId = p.ProfileId, Count = p.Count });

        return await aggregation.ToListAsync();
    }
}