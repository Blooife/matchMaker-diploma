namespace Match.DataAccess.Providers.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMatchRepository Matches { get; }
    IProfileRepository Profiles { get; }
    ILikeRepository Likes { get; }
    IChatRepository Chats { get; }
    IMessageRepository Messages { get; }
}
