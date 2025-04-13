using Common.Models;
using Match.BusinessLogic.DTOs.Message;

namespace Match.BusinessLogic.Services.Interfaces;

public interface IMessageService
{
    Task<PagedList<MessageResponseDto>> GetMessagesByChatIdAsync(
        long chatId, int pageNumber, int pageSize, CancellationToken cancellationToken);
}