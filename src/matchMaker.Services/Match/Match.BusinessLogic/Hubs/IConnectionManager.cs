namespace Match.BusinessLogic.Hubs;

public interface IConnectionManager
{
    void AddUserToChat(string chatId, long userId);
    void RemoveUserFromChat(string chatId, long userId);
    bool IsUserInChat(string chatId, long userId);
}
