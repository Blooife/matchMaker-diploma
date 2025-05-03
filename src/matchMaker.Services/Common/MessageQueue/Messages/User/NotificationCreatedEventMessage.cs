using Common.Enums;
using MessageQueue.MessageContracts;

namespace MessageQueue.Messages.User;

public class NotificationCreatedEventMessage : IAsyncCommunicationEvent
{
    public long UserId { get; set; }
    public string Body { get; set; }
    public NotificationType NotificationType { get; set; }
    public string RoutingKey { get; set; } = "notification.created";
}