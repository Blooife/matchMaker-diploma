using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }
    public long SenderId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public long ChatId { get; set; }
}