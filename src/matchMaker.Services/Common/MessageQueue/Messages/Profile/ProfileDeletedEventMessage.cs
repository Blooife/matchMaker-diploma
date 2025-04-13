using MessageQueue.MessageContracts;

namespace MessageQueue.Messages.Profile;

public class ProfileDeletedEventMessage : IAsyncCommunicationEvent
{
    public long Id { get; set; }
    public string RoutingKey { get; set; } = "profile.deleted";
}