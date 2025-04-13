using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class Chat
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }
    public long FirstProfileId { get; set; }
    public long SecondProfileId { get; set; }
    public DateTime LastMessageTimestamp { get; set; }
}