using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Match.BusinessLogic.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly Dictionary<string, HashSet<long>> _activeUsersInChat = new();

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task SendMessage(string chatId, long senderId, string message)
    {
        var newMessage = await _chatService.SendMessageAsync(chatId, senderId, message);

        var chat = await _chatService.GetById(chatId);
        var receiverId = senderId == chat.FirstProfileId ? chat.SecondProfileId : chat.FirstProfileId;

        var isReceiverActive = _activeUsersInChat.ContainsKey(chatId) && _activeUsersInChat[chatId].Contains(receiverId);

        if (!isReceiverActive)
        {
            var unreadCount = await _chatService.IncrementUnreadCountAsync(chatId, receiverId);

            await Clients.User(receiverId.ToString())
                .SendAsync("UpdateUnreadCount", chatId, unreadCount);
        }
        
        await Clients.Group(chatId).SendAsync("ReceiveMessage", newMessage);
    }

    public async Task JoinChat(string chatId, long profileId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        if (!_activeUsersInChat.ContainsKey(chatId))
            _activeUsersInChat[chatId] = new HashSet<long>();

        _activeUsersInChat[chatId].Add(profileId);
    }

    public async Task LeaveChat(string chatId, long profileId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

        if (_activeUsersInChat.ContainsKey(chatId))
            _activeUsersInChat[chatId].Remove(profileId);
    }

}