using Match.BusinessLogic.DTOs.Notification;

namespace Match.BusinessLogic.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationResponseDto>> GetUserNotificationsAsync(long profileId, CancellationToken cancellationToken);
    Task MarkNotificationsAsReadAsync(string[] ids, CancellationToken cancellationToken);
    Task CreateNewMessageNotificationAsync(
        long profileId, string chatId, long senderId, string senderName, CancellationToken cancellationToken);
}