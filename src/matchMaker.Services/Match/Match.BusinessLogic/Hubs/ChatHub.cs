using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Match.BusinessLogic.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task SendMessage(long chatId, long senderId, string message)
    {
        var newMessage = await _chatService.SendMessageAsync(chatId, senderId, message);
        
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", newMessage);
    }

    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}