using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public long SenderId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string ChatId { get; set; }
}