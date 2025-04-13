using Common.Models;
using Match.BusinessLogic.DTOs.Chat;
using Match.DataAccess.Models;

namespace Match.BusinessLogic.Services.Interfaces;

public interface IChatService
{
    Task<Message> SendMessageAsync(long chatId, long senderId, string message, CancellationToken cancellationToken = default);
    Task<ChatResponseDto> GetChatsByProfileIdsAsync(
        long firstProfileId, long secondProfileId, CancellationToken cancellationToken);
    Task<PagedList<ChatResponseDto>> GetChatsByProfileId(
        long profileId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<ChatResponseDto> CreateChatAsync(long firstProfileId, long secondProfileId, CancellationToken cancellationToken);
    Task<GeneralResponseDto> DeleteChatAsync(long chatId, CancellationToken cancellationToken);
}