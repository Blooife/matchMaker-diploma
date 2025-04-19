using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class Like
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public long ProfileId { get; set; }
    public long TargetProfileId { get; set; }
    public bool IsLike { get; set; }
}