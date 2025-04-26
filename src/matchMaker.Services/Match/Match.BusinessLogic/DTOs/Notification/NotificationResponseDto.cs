using Common.Enums;

namespace Match.BusinessLogic.DTOs.Notification;

public class NotificationResponseDto
{
    
    public string Id { get; set; }
    public long ProfileId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public string? ChatId { get; set; }
    public long SenderId { get; set; }
}