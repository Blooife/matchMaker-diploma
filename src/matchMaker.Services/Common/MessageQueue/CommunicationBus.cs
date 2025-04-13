using MassTransit;
using MessageQueue.MessageContracts;
using Microsoft.Extensions.Logging;

namespace MessageQueue;

public class CommunicationBus(IPublishEndpoint bus, ILogger<CommunicationBus> logger) : ICommunicationBus
{
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
    where T : IAsyncCommunicationEvent
    {
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
        try
        {
            await bus.Publish(@event, linkedCts.Token);

            logger.LogInformation("Сообщение успешно опубликовано в брокер. Тип сообщения: {MessageType}", typeof(T).Name);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            logger.LogError("Превышено время ожидания публикации сообщения типа {MessageType}", typeof(T).Name);
        }
        catch (RabbitMqConnectionException ex)
        {
            logger.LogError(ex, "Брокер недоступен. Не удалось опубликовать сообщение типа {MessageType}", typeof(T).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при попытке опубликовать сообщение типа {MessageType}", typeof(T).Name);
        }
    }

    public async Task PublishBatchAsync<T>(ICollection<T> events, CancellationToken cancellationToken = default)
        where T : IAsyncCommunicationEvent
    {
        if (events == null || events.Count == 0)
        {
            return;
        }

        foreach (var @event in events)
        {
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            try
            {
                await bus.Publish(@event, linkedCts.Token);

                logger.LogInformation("Сообщение успешно опубликовано в брокер. Тип сообщения: {MessageType}", typeof(T).Name);
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
            {
                logger.LogError("Превышено время ожидания публикации сообщения типа {MessageType}", typeof(T).Name);
            }
            catch (RabbitMqConnectionException ex)
            {
                logger.LogError(ex, "Брокер недоступен. Не удалось опубликовать сообщение типа {MessageType}", typeof(T).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при попытке опубликовать сообщение типа {MessageType}", typeof(T).Name);
            }
        }
    }
}