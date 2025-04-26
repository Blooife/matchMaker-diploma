namespace Match.DataAccess.Context;

public class MatchDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string LikesCollectionName { get; set; } = null!;
    public string MatchesCollectionName { get; set; } = null!;
    public string ProfilesCollectionName { get; set; } = null!;
    public string ChatsCollectionName { get; set; } = null!;
    public string MessagesCollectionName { get; set; } = null!;
    public string NotificationsCollectionName { get; set; } = null!;
}