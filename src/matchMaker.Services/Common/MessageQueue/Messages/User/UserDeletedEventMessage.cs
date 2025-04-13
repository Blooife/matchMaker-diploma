using MessageQueue.MessageContracts;

namespace MessageQueue.Messages.User;

public class UserDeletedEventMessage : IAsyncCommunicationEvent
{
    public long Id { get; set; }
    public string RoutingKey { get; set; } = "user.deletec";
}