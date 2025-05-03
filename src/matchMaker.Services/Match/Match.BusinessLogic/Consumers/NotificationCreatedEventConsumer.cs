using AutoMapper;
using Common.Enums;
using MassTransit;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MessageQueue.Messages.User;

namespace Match.BusinessLogic.Consumers;

public class NotificationCreatedEventConsumer(IUnitOfWork _unitOfWork) : IConsumer<NotificationCreatedEventMessage>
{

    public async Task Consume(ConsumeContext<NotificationCreatedEventMessage> context)
    {
        var message = context.Message;

        if (message.NotificationType == NotificationType.NewReportOnYou)
        {
            var notification = new Notification
            {
                ProfileId = message.UserId,
                Title = "Новая жалоба",
                Body = message.Body,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationType.NewReportOnYou,
                ChatId = null,
                SenderId = null
            };

            await _unitOfWork.Notifications.CreateAsync(notification, context.CancellationToken);
        }
    }
}