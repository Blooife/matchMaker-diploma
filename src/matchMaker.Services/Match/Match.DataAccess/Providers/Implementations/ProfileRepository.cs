using Common.Constants;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Implementations;

public class ProfileRepository(IMongoCollection<Profile> _collection) : GenericRepository<Profile, long>(_collection), IProfileRepository
{
    public async Task<List<long>> GetRecsAsync(List<long> excludedProfileIds, Profile userProfile, CancellationToken cancellationToken)
    {
        var filter = GetFilterForRecommendations(excludedProfileIds, userProfile, false);
        
        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<Profile, Profile>()
        {
            Limit = 10,
            Projection = Builders<Profile>.Projection.Include(p => p.Id)
        };

        var ids = await _collection.Find(filter)
            .Project(p=>p.Id)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        if (ids.Count == 0)
        {
            filter = GetFilterForRecommendations(excludedProfileIds, userProfile, true);

            ids = await _collection.Find(filter)
                .Project(p => p.Id)
                .Limit(10)
                .ToListAsync(cancellationToken);
        }
        
        return ids;
    }

    private FilterDefinition<Profile> GetFilterForRecommendations(List<long> excludedProfileIds, Profile userProfile, bool filterByCountry)
    {
        var filters = new List<FilterDefinition<Profile>>
        {
            Builders<Profile>.Filter.Ne(p => p.Id, userProfile.Id),
            Builders<Profile>.Filter.Nin(p => p.Id, excludedProfileIds), 
        };

        if (userProfile.PreferredGender != Gender.Undefined)
        {
            filters.Add(Builders<Profile>.Filter.Eq(p => p.Gender, userProfile.PreferredGender));
        }

        filters.Add(Builders<Profile>.Filter.And(
            Builders<Profile>.Filter.Gte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeTo)),
            Builders<Profile>.Filter.Lte(p => p.BirthDate, DateTime.Now.AddYears(-userProfile.AgeFrom))
        ));

        if (userProfile is { Location: not null, MaxDistance: > 0 })
        {
            var coordinates = userProfile.Location.Coordinates;
            filters.Add(Builders<Profile>.Filter.GeoWithinCenterSphere(p=>p.Location, coordinates.X, coordinates.Y, userProfile.MaxDistance / 6371.0 ));
        }
        else
        {
            if (filterByCountry && !string.IsNullOrEmpty(userProfile.Country))
            {
                filters.Add(Builders<Profile>.Filter.Eq(p => p.Country, userProfile.Country));
            }
            else
            {
                filters.Add(Builders<Profile>.Filter.Eq(p => p.City, userProfile.City));
            }
        }
        
        var resFilter = ApplySoftDeleteFilter(Builders<Profile>.Filter.And(filters));
        
        return resFilter;
    }
}