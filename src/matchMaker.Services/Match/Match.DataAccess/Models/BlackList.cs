using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class BlackList
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public long BlockerProfileId { get; set; }
    public long BlockedProfileId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}