using Common.Constants;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Implementations;

public class ProfileRepository(IMongoCollection<Profile> _collection) : GenericRepository<Profile, long>(_collection), IProfileRepository
{
    public async Task<List<long>> GetRecsAsync(List<long> excludedProfileIds, Profile userProfile, CancellationToken cancellationToken, int? limit = null)
    {
        var filter = GetFilterForRecommendations(excludedProfileIds, userProfile, false);
        var ids = await ApplyFindWithLimit(filter);

        if (ids.Count == 0)
        {
            filter = GetFilterForRecommendations(excludedProfileIds, userProfile, true);
            ids = await ApplyFindWithLimit(filter);
        }

        if (ids.Count == 0)
        {
            filter = GetFilterForRecommendations(excludedProfileIds, userProfile, null);
            ids = await ApplyFindWithLimit(filter);
        }

        return ids;

        async Task<List<long>> ApplyFindWithLimit(FilterDefinition<Profile> filter)
        {
            var query = _collection.Find(filter).Project(p => p.Id);
            if (limit.HasValue)
            {
                query = query.Limit(limit.Value);
            }
            return await query.ToListAsync(cancellationToken);
        }
    }

    private FilterDefinition<Profile> GetFilterForRecommendations(List<long> excludedProfileIds, Profile userProfile, bool? filterByCountry)
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

        var geoFilter = GeoFilterStrategy.Build(userProfile, filterByCountry);
        if (geoFilter != Builders<Profile>.Filter.Empty)
        {
            filters.Add(geoFilter);
        }
        
        var resFilter = ApplySoftDeleteFilter(Builders<Profile>.Filter.And(filters));
        
        return resFilter;
    }
    
    private static class GeoFilterStrategy
    {
        public static FilterDefinition<Profile> Build(
            Profile userProfile,
            bool? filterByCountry
        )
        {
            var filters = new List<FilterDefinition<Profile>>();

            if (userProfile.Location != null && userProfile.MaxDistance > 0)
            {
                var coordinates = userProfile.Location.Coordinates;
                filters.Add(Builders<Profile>.Filter.GeoWithinCenterSphere(
                    p => p.Location,
                    coordinates.X,
                    coordinates.Y,
                    userProfile.MaxDistance / 6371.0
                ));
            }
            else if (filterByCountry.HasValue)
            {
                if (filterByCountry.Value && !string.IsNullOrEmpty(userProfile.Country))
                {
                    filters.Add(Builders<Profile>.Filter.Eq(p => p.Country, userProfile.Country));
                }
                else
                {
                    filters.Add(Builders<Profile>.Filter.Eq(p => p.City, userProfile.City));
                }
            }

            return filters.Count > 0
                ? Builders<Profile>.Filter.And(filters)
                : Builders<Profile>.Filter.Empty;
        }
    }
}