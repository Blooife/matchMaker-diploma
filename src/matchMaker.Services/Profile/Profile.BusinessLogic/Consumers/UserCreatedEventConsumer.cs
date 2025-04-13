using MassTransit;
using MessageQueue.Messages.User;
using Microsoft.Extensions.Logging;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.BusinessLogic.Consumers;

public class UserCreatedEventConsumer(IUnitOfWork _unitOfWork, ILogger<UserCreatedEventConsumer> _logger) : IConsumer<UserCreatedEventMessage>
{
    public async Task Consume(ConsumeContext<UserCreatedEventMessage> context)
    {
        var eventMessage = context.Message;
        _logger.LogInformation($"UserCreatedEventConsumer consumed message: {eventMessage.Id}");
        await _unitOfWork.UserRepository.CreateUserAsync(new DataAccess.Models.User(){Id = eventMessage.Id, Email = eventMessage.Email }, cancellationToken: context.CancellationToken);
        await _unitOfWork.SaveAsync(context.CancellationToken);
    }
}