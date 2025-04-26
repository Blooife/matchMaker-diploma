using System.Collections.Concurrent;

namespace Match.BusinessLogic.Hubs;

public class ConnectionManager : IConnectionManager
{
    private readonly ConcurrentDictionary<string, HashSet<long>> _chatUsers = new();

    public void AddUserToChat(string chatId, long userId)
    {
        _chatUsers.AddOrUpdate(chatId,
            _ => new HashSet<long> { userId },
            (_, existing) =>
            {
                existing.Add(userId);
                return existing;
            });
    }

    public void RemoveUserFromChat(string chatId, long userId)
    {
        if (_chatUsers.TryGetValue(chatId, out var users))
        {
            users.Remove(userId);
        }
    }

    public bool IsUserInChat(string chatId, long userId)
    {
        return _chatUsers.TryGetValue(chatId, out var users) && users.Contains(userId);
    }
}
