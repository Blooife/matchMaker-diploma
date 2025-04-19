using Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class MatchEntity : ISoftDeletable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public long FirstProfileId { get; set; }
    public long SecondProfileId { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime? DeletedAt { get; set; }
}