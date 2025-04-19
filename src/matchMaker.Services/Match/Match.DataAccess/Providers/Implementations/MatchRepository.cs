using System.Linq.Expressions;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;
using Match.DataAccess.Models;

namespace Match.DataAccess.Providers.Implementations;

public class MatchRepository(IMongoCollection<MatchEntity> _collection) : GenericRepository<MatchEntity, string>(_collection), IMatchRepository
{
    public async Task<bool> AreProfilesMatchedAsync(long profileId1, long profileId2, CancellationToken cancellationToken)
    {
        var filter = Builders<MatchEntity>.Filter.Or(
            Builders<MatchEntity>.Filter.And(
                Builders<MatchEntity>.Filter.Eq(match => match.FirstProfileId, profileId1),
                Builders<MatchEntity>.Filter.Eq(match => match.SecondProfileId, profileId2)
            ),
            Builders<MatchEntity>.Filter.And(
                Builders<MatchEntity>.Filter.Eq(match => match.FirstProfileId, profileId2),
                Builders<MatchEntity>.Filter.Eq(match => match.SecondProfileId, profileId1)
            )
        );

        var resFilter = ApplySoftDeleteFilter(filter);
        
        return await _collection.Find(resFilter).AnyAsync(cancellationToken);
    }
    
    public async Task<(List<MatchEntity>, int)> GetPagedAsync(long profileId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        Expression<Func<MatchEntity, bool>> filter = match => match.FirstProfileId == profileId || match.SecondProfileId == profileId;
        var resFilter = ApplySoftDeleteFilter(filter);
        var count = await _collection.CountDocumentsAsync(resFilter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<MatchEntity, MatchEntity>()
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize,
            Sort = Builders<MatchEntity>.Sort.Descending(match => match.Timestamp)
        };

        var items = await _collection.Find(resFilter)
            .Sort(findOptions.Sort)
            .Skip(findOptions.Skip)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return (items, (int)count);
    }
}