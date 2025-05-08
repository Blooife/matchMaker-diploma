using AutoMapper;
using Common.Authorization.Context;
using Common.Enums;
using Match.BusinessLogic.DTOs.Notification;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.BusinessLogic.Services.Implementations;

public class NotificationService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthenticationContext _authenticationContext) : INotificationService
{
    public async Task<List<NotificationResponseDto>> GetUserNotificationsAsync(CancellationToken cancellationToken)
    {
        var notifications = (await _unitOfWork.Notifications.GetAsync(
            n => n.ProfileId == _authenticationContext.UserId,
            cancellationToken
            ))
            .OrderByDescending(n => n.CreatedAt)
            .ToList();

        return _mapper.Map<List<NotificationResponseDto>>(notifications);
    }

    public async Task MarkNotificationsAsReadAsync(string[] ids, CancellationToken cancellationToken)
    {
        var update = Builders<Notification>.Update.Set(n => n.IsRead, true);

        await _unitOfWork.Notifications.UpdateManyAsync(
            n => ids.Contains(n.Id) && n.ProfileId == _authenticationContext.UserId,
            update,
            cancellationToken
        );
    }
    
    public async Task CreateNewMessageNotificationAsync(
        long profileId, string chatId, long senderId, string senderName, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            ProfileId = profileId,
            Title = "Новое сообщение",
            Body = $"Новое сообщение от {senderName}",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            Type = NotificationType.NewMessage,
            ChatId = chatId,
            SenderId = senderId
        };

        await _unitOfWork.Notifications.CreateAsync(notification, cancellationToken);
    }

}