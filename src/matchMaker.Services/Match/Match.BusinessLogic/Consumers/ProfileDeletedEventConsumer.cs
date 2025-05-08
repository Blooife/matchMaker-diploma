using Common.Exceptions;
using MassTransit;
using Match.DataAccess.Providers.Interfaces;
using MessageQueue.Messages.Profile;

namespace Match.BusinessLogic.Consumers;

public class ProfileDeletedEventConsumer(IUnitOfWork _unitOfWork) : IConsumer<ProfileDeletedEventMessage>
{

    public async Task Consume(ConsumeContext<ProfileDeletedEventMessage> context)
    {
        var eventMessage = context.Message;
        var cancellationToken = context.CancellationToken;
        
        var profile = await _unitOfWork.Profiles.GetByIdAsync(eventMessage.Id, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Профиль");
        }
        
        await _unitOfWork.Profiles.DeleteAsync(profile, cancellationToken);
        
        await _unitOfWork.Chats.DeleteManyAsync(
            chat => chat.FirstProfileId == profile.Id || chat.SecondProfileId == profile.Id, cancellationToken);
        
        var chats = await _unitOfWork.Chats.GetChatsByProfileIdAsync(profile.Id, cancellationToken);
        
        var chatIds = chats.Select(c => c.Id).ToList();
        
        await _unitOfWork.Messages.DeleteManyAsync(message => chatIds.Contains(message.ChatId), cancellationToken);
        
        await _unitOfWork.Matches.DeleteManyAsync(
            match => match.FirstProfileId == profile.Id || match.SecondProfileId == profile.Id, cancellationToken);
        
        await _unitOfWork.Likes.DeleteManyAsync(
            like => like.ProfileId== profile.Id || like.TargetProfileId == profile.Id, cancellationToken);
    }
}