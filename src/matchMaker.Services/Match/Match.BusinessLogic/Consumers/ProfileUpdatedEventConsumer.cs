using AutoMapper;
using Common.Exceptions;
using MassTransit;
using Match.DataAccess.Providers.Interfaces;
using MessageQueue.Messages.Profile;
using Microsoft.Extensions.Logging;

namespace Match.BusinessLogic.Consumers;

public class ProfileUpdatedEventConsumer(
    IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<ProfileUpdatedEventConsumer> _logger) : IConsumer<ProfileUpdatedEventMessage>
{
    public async Task Consume(ConsumeContext<ProfileUpdatedEventMessage> context)
    {
        var eventMessage = context.Message;
        var cancellationToken = context.CancellationToken;
        
        var profileMapped = _mapper.Map<DataAccess.Models.Profile>(eventMessage);
        
        var profile = await _unitOfWork.Profiles.GetByIdAsync(profileMapped.Id, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Профиль");
        }
        
        await _unitOfWork.Profiles.UpdateAsync(profileMapped, cancellationToken);
    }
}