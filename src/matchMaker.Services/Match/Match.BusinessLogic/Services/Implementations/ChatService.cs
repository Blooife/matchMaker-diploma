using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Match.BusinessLogic.DTOs.Chat;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Providers.Interfaces;
using Match.DataAccess.Models;

namespace Match.BusinessLogic.Services.Implementations;

public class ChatService(IUnitOfWork _unitOfWork, IMapper _mapper) : IChatService
{
    public async Task<Message> SendMessageAsync(string chatId, long senderId, string message, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, cancellationToken);
        
        if (chat is null)
        {
            throw new NotFoundException(chatId);
        }

        var newMessage = new Message
        {
            SenderId = senderId,
            Content = message,
            Timestamp = DateTime.UtcNow,
            ChatId = chat.Id
        };

        await _unitOfWork.Messages.CreateAsync(newMessage, cancellationToken);

        chat.LastMessageTimestamp = newMessage.Timestamp;
        
        await _unitOfWork.Chats.UpdateAsync(chat, cancellationToken);
        
        return newMessage;
    }
    
    public async Task<ChatResponseDto> GetChatsByProfileIdsAsync(
        long firstProfileId, long secondProfileId, CancellationToken cancellationToken)
    {
        var profile1 = await _unitOfWork.Profiles.GetByIdAsync(firstProfileId, cancellationToken);

        if (profile1 is null)
        {
            throw new NotFoundException(firstProfileId);
        }
        
        var profile2 = await _unitOfWork.Profiles.GetByIdAsync(secondProfileId, cancellationToken);

        if (profile2 is null)
        {
            throw new NotFoundException(secondProfileId);
        }

        var chat = await _unitOfWork.Chats.GetChatByProfilesIdsAsync(firstProfileId, secondProfileId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException($"Chat with profile ids: {firstProfileId} and {secondProfileId} wad not found");
        }

        var mappedChat = _mapper.Map<ChatResponseDto>(chat);
        mappedChat.ProfileName = mappedChat.FirstProfileId == profile1.Id ? profile2.Name : profile1.Name;
        mappedChat.ProfileLastName = mappedChat.FirstProfileId == profile1.Id ? profile2.LastName : profile1.LastName;
        mappedChat.MainImageUrl = mappedChat.FirstProfileId == profile1.Id ? profile2.MainImageUrl : profile1.MainImageUrl;
        
        return mappedChat;
    }
    
    public async Task<PagedList<ChatResponseDto>> GetChatsByProfileId(
        long profileId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException(profileId);
        }

        var (chats, count) = await _unitOfWork.Chats.GetPagedAsync(profileId, pageNumber, pageSize,
            cancellationToken);
        
        var profileIds = chats
            .SelectMany(chat => new[] { chat.FirstProfileId, chat.SecondProfileId })
            .Distinct()
            .ToList();
        
        var profiles = await _unitOfWork.Profiles.GetAsync(p => profileIds.Contains(p.Id), cancellationToken);

        var profileDictionary = profiles.ToDictionary(p => p.Id);
        
        var chatResponseDtos = chats.Select(chat =>
        {
            var otherProfileId = chat.FirstProfileId == profileId ? chat.SecondProfileId : chat.FirstProfileId;
            var otherProfile = profileDictionary[otherProfileId];
            var isFirstRequested = chat.FirstProfileId == profileId;

            return new ChatResponseDto
            {
                Id = chat.Id,
                FirstProfileId = chat.FirstProfileId,
                SecondProfileId = chat.SecondProfileId,
                ProfileName = otherProfile.Name,
                ProfileLastName = otherProfile.LastName,
                MainImageUrl = otherProfile.MainImageUrl,
                RequestedProfileUnreadCount = isFirstRequested
                    ? chat.FirstProfileUnreadCount
                    : chat.SecondProfileUnreadCount, 
                ReceiverProfileUnreadCount = isFirstRequested
                    ? chat.SecondProfileUnreadCount
                    : chat.FirstProfileUnreadCount,
            };
        }).ToList();

        return new PagedList<ChatResponseDto>(chatResponseDtos, count, pageNumber, pageSize);
    }
    
    public async Task<ChatResponseDto> CreateChatAsync(long firstProfileId, long secondProfileId, CancellationToken cancellationToken)
    {
        var profile1 = await _unitOfWork.Profiles.GetByIdAsync(firstProfileId, cancellationToken);

        if (profile1 is null)
        {
            throw new NotFoundException(firstProfileId);
        }
        
        var profile2 = await _unitOfWork.Profiles.GetByIdAsync(secondProfileId, cancellationToken);

        if (profile2 is null)
        {
            throw new NotFoundException(secondProfileId);
        } 
        
        var areProfilesMatched =
            await _unitOfWork.Matches.AreProfilesMatchedAsync(firstProfileId, secondProfileId, cancellationToken);
        
        if (!areProfilesMatched)
        {
            throw new ProfilesAreNotMatchedException();
        }
        
        var chat = new Chat()
        {
            FirstProfileId = firstProfileId,
            SecondProfileId = secondProfileId,
            FirstProfileUnreadCount = 0,
            SecondProfileUnreadCount = 0,
        };
        
        await _unitOfWork.Chats.CreateAsync(chat, cancellationToken);
        
        var mappedChat = _mapper.Map<ChatResponseDto>(chat);
        mappedChat.ProfileName = mappedChat.FirstProfileId == profile1.Id ? profile2.Name : profile1.Name;
        mappedChat.ProfileLastName = mappedChat.FirstProfileId == profile1.Id ? profile2.LastName : profile1.LastName;
        mappedChat.MainImageUrl = mappedChat.FirstProfileId == profile1.Id ? profile2.MainImageUrl : profile1.MainImageUrl;
        
        return mappedChat;
    }
    
    public async Task<GeneralResponseDto> DeleteChatAsync(string chatId, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException(chatId);
        }

        await _unitOfWork.Chats.DeleteAsync(chat, cancellationToken);
        
        await _unitOfWork.Messages.DeleteManyAsync(message => message.ChatId == chat.Id, cancellationToken);
        
        return new GeneralResponseDto();
    }
    
    public async Task ReadChatAsync(ReadChatDto dto, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(dto.ChatId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException(dto.ChatId);
        }
        
        if (dto.ProfileId == chat.FirstProfileId)
        {
            chat.FirstProfileUnreadCount = 0;
        }
        else
        {
            chat.SecondProfileUnreadCount = 0;
        }
        
        await _unitOfWork.Chats.UpdateAsync(chat, cancellationToken);
    }

    public async Task<Chat> GetById(string id)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(id, CancellationToken.None);

        if (chat is null)
        {
            throw new NotFoundException(id);
        }

        return chat;
    }

    public async Task<ChatResponseDto> IncrementUnreadCountAsync(string chatId, long receiverId)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, CancellationToken.None);

        if (chat is null)
        {
            throw new NotFoundException(chatId);
        }
        
        long newCount = 0;

        if (receiverId == chat.FirstProfileId)
        {
            chat.FirstProfileUnreadCount++;
        }
        else
        {
            chat.SecondProfileUnreadCount++;
        }

        await _unitOfWork.Chats.UpdateAsync(chat, CancellationToken.None);
        
        var isFirstRequested = chat.SecondProfileId == receiverId;
        return new ChatResponseDto
        {
            Id = chat.Id,
            FirstProfileId = chat.FirstProfileId,
            SecondProfileId = chat.SecondProfileId,
            RequestedProfileUnreadCount = isFirstRequested
                ? chat.FirstProfileUnreadCount
                : chat.SecondProfileUnreadCount, 
            ReceiverProfileUnreadCount = isFirstRequested
                ? chat.SecondProfileUnreadCount
                : chat.FirstProfileUnreadCount,
        };;
    }
}