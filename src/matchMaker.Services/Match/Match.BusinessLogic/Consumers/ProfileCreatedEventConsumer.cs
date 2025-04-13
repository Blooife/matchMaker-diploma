using AutoMapper;
using Common.Exceptions;
using MassTransit;
using Match.DataAccess.Providers.Interfaces;
using MessageQueue.Messages.Profile;

namespace Match.BusinessLogic.Consumers;

public class ProfileCreatedEventConsumer(IUnitOfWork _unitOfWork, IMapper _mapper) : IConsumer<ProfileCreatedEventMessage>
{

    public async Task Consume(ConsumeContext<ProfileCreatedEventMessage> context)
    {
        var eventMessage = context.Message;
        
        var existingProfile = await _unitOfWork.Profiles.GetByIdAsync(eventMessage.Id, context.CancellationToken);
        
        if (existingProfile is not null)
        {
            throw new AlreadyExistsException("Profile already exists in database");
        }
        
        var profile = _mapper.Map<DataAccess.Models.Profile>(eventMessage);
        
        await _unitOfWork.Profiles.CreateAsync(profile, context.CancellationToken);
    }
}