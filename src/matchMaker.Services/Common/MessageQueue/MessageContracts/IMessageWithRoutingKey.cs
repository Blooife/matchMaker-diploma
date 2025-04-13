namespace MessageQueue.MessageContracts;

public interface IMessageWithRoutingKey
{
    public string RoutingKey { get; set; }
}