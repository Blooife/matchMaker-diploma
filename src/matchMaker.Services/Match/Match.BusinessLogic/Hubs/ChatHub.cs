using Match.BusinessLogic.DTOs.Chat;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Match.BusinessLogic.Hubs;

public class ChatHub(
    IChatService _chatService,
    IConnectionManager _connectionManager,
    INotificationService _notificationService,
    IProfileService _profileService) : Hub
{
    public async Task SendMessage(string chatId, long senderId, string message)
    {
        var newMessage = await _chatService.SendMessageAsync(chatId, senderId, message);

        var chat = await _chatService.GetById(chatId);
        var receiverId = senderId == chat.FirstProfileId ? chat.SecondProfileId : chat.FirstProfileId;

        var isReceiverActive = _connectionManager.IsUserInChat(chatId, receiverId);

        if (!isReceiverActive)
        {
            var chatUnreadCount = await _chatService.IncrementUnreadCountAsync(chatId, receiverId);

            await Clients.User(senderId.ToString())
                .SendAsync("UpdateUnreadCount", chatId, chatUnreadCount.RequestedProfileUnreadCount, chatUnreadCount.ReceiverProfileUnreadCount);
            await Clients.User(receiverId.ToString())
                .SendAsync("UpdateUnreadCount", chatId, chatUnreadCount.ReceiverProfileUnreadCount, chatUnreadCount.RequestedProfileUnreadCount);
            
            var senderProfile = await _profileService.GetByIdAsync(senderId, CancellationToken.None);
            await _notificationService.CreateNewMessageNotificationAsync(
                receiverId, chatId, senderId, $"{senderProfile.LastName} {senderProfile.Name}", CancellationToken.None);
        }
        
        await Clients.Group(chatId).SendAsync("ReceiveMessage", newMessage);
    }

    public async Task JoinChat(string chatId, long profileId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        _connectionManager.AddUserToChat(chatId, profileId);
    }

    public async Task LeaveChat(string chatId, long profileId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

        _connectionManager.RemoveUserFromChat(chatId, profileId);
    }

    public async Task ReadChat(string chatId, string profileId)
    {
        await _chatService.ReadChatAsync(new ReadChatDto
        {
            ChatId = chatId,
            ProfileId = int.Parse(profileId)
        }, CancellationToken.None);

        await Clients.Group(chatId)
            .SendAsync("MessagesRead", chatId, profileId);
    }
}