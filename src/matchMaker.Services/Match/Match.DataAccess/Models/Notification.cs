using Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Match.DataAccess.Models;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public long ProfileId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ChatId { get; set; }
    public long SenderId { get; set; }
}