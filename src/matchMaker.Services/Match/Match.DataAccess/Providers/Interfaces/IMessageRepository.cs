using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.DataAccess.Providers.Interfaces;

public interface IMessageRepository : IGenericRepository<Message, long>
{
    Task<(List<Message>, int)> GetPagedAsync(long chatId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}