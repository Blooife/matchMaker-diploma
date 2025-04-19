using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.DataAccess.Providers.Interfaces;

public interface IMessageRepository : IGenericRepository<Message, string>
{
    Task<(List<Message>, int)> GetPagedAsync(string chatId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}