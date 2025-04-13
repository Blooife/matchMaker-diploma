using MessageQueue.MessageContracts;

namespace MessageQueue;

public interface ICommunicationBus
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : IAsyncCommunicationEvent;

    Task PublishBatchAsync<T>(ICollection<T> events, CancellationToken cancellationToken = default)
        where T : IAsyncCommunicationEvent;
}