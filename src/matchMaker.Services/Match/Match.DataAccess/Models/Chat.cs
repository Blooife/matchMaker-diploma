using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class Chat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public long FirstProfileId { get; set; }
    public long SecondProfileId { get; set; }
    public long FirstProfileUnreadCount { get; set; }
    public long SecondProfileUnreadCount { get; set; }
    public DateTime LastMessageTimestamp { get; set; }
}