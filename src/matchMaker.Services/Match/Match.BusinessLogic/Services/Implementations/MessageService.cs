using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Match.BusinessLogic.DTOs.Message;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Providers.Interfaces;

namespace Match.BusinessLogic.Services.Implementations;

public class MessageService(IUnitOfWork _unitOfWork, IMapper _mapper) : IMessageService
{
    public async Task<PagedList<MessageResponseDto>> GetMessagesByChatIdAsync(
        string chatId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException("Чат");
        }

        var (messages, count) =
            await _unitOfWork.Messages.GetPagedAsync(chatId, pageNumber, pageSize, cancellationToken);
        
        var mappedMessages = _mapper.Map<List<MessageResponseDto>>(messages);

        return new PagedList<MessageResponseDto>(mappedMessages, count, pageNumber, pageSize);
    }
}