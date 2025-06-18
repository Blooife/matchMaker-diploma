using Common.Constants;
using Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Match.DataAccess.Models;

public class Profile : ISoftDeletable
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
    public long CountryId { get; set; }
    public string Country { get; set; }
    public long CityId { get; set; }
    public string City { get; set; }
    public string MainImageUrl { get; set; }
    public GeoJsonPoint<GeoJson2DCoordinates>? Location { get; set; }
    public DateTime? DeletedAt { get; set; }
}