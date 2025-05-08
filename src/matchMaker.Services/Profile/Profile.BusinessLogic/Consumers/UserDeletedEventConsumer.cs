using Common.Exceptions;
using MassTransit;
using MessageQueue;
using MessageQueue.Messages.Profile;
using MessageQueue.Messages.User;
using Microsoft.Extensions.Logging;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.BusinessLogic.Consumers;

public class UserDeletedEventConsumer(IUnitOfWork _unitOfWork, ICommunicationBus _communicationBus, ILogger<UserDeletedEventConsumer> _logger) : IConsumer<UserDeletedEventMessage>
{
    public async Task Consume(ConsumeContext<UserDeletedEventMessage> context)
    {
        var eventMessage = context.Message;
        _logger.LogInformation($"UserDeletedEventConsumer consumed message: {eventMessage.Id}");
        var user = (await _unitOfWork.UserRepository.GetAsync(x => x.Id == eventMessage.Id, context.CancellationToken)).FirstOrDefault();

        if (user is null)
        {
            throw new NotFoundException("Пользователь");
        }
        
        await _unitOfWork.UserRepository.DeleteUserAsync(user);
        await _unitOfWork.SaveAsync(context.CancellationToken);
        
        var profile = (await _unitOfWork.ProfileRepository
            .GetAsync(x => x.UserId == user.Id, context.CancellationToken)).FirstOrDefault();

        if (profile is not null)
        {
            await _communicationBus.PublishAsync(new ProfileDeletedEventMessage() { Id = profile.Id }, context.CancellationToken);
        }
    }
}