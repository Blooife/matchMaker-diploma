using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class Like
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }
    public long ProfileId { get; set; }
    public long TargetProfileId { get; set; }
    public bool IsLike { get; set; }
}