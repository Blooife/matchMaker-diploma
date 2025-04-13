using MessageQueue.MessageContracts;

namespace MessageQueue.Messages.User;

public class UserCreatedEventMessage : IAsyncCommunicationEvent
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string RoutingKey { get; set; } = "user.created";
}