using AutoMapper;
using Common.Authorization.Context;
using Common.Exceptions;
using Common.Models;
using Match.BusinessLogic.DTOs.Chat;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Providers.Interfaces;
using Match.DataAccess.Models;

namespace Match.BusinessLogic.Services.Implementations;

public class ChatService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthenticationContext _authenticationContext) : IChatService
{
    public async Task<Message> SendMessageAsync(string chatId, long senderId, string message, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, cancellationToken);
        
        if (chat is null)
        {
            throw new NotFoundException("Чат");
        }
        
        var receiverId = senderId == chat.FirstProfileId ? chat.SecondProfileId : chat.FirstProfileId;
        await _unitOfWork.BlackLists.CheckCanSendMessageAsync(senderId, receiverId);
        
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
        var profile1 = await _unitOfWork.Profiles.GetByIdAsync(firstProfileId, cancellationToken)
                       ?? throw new NotFoundException("Профиль");

        var profile2 = await _unitOfWork.Profiles.GetByIdAsync(secondProfileId, cancellationToken)
                       ?? throw new NotFoundException("Профиль");

        var chat = await _unitOfWork.Chats.GetChatByProfilesIdsAsync(firstProfileId, secondProfileId, cancellationToken)
                   ?? throw new NotFoundException($"Чат");

        return await MapToChatResponseDtoAsync(chat, _authenticationContext.UserId, cancellationToken);
    }
    
    public async Task<PagedList<ChatResponseDto>> GetChatsByProfileId(
        int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var profileId = _authenticationContext.UserId;
        var profile = await _unitOfWork.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException("Профиль");
        }

        var (chats, count) = await _unitOfWork.Chats.GetPagedAsync(profileId, pageNumber, pageSize,
            cancellationToken);
        
        var profileIds = chats
            .SelectMany(chat => new[] { chat.FirstProfileId, chat.SecondProfileId })
            .Distinct()
            .ToList();
        
        var profiles = await _unitOfWork.Profiles.GetAsync(p => profileIds.Contains(p.Id), cancellationToken);

        var profileDictionary = profiles.ToDictionary(p => p.Id);
        
        var blockerBlackList = await _unitOfWork.BlackLists.GetByBlockerIdAsync(_authenticationContext.UserId, cancellationToken);
        var blockedBlackList = await _unitOfWork.BlackLists.GetByBlockedIdAsync(_authenticationContext.UserId, cancellationToken);
        
        var chatResponseDtos = chats.Select(chat =>
        {
            var otherProfileId = chat.FirstProfileId == profileId ? chat.SecondProfileId : chat.FirstProfileId;
            var otherProfile = profileDictionary[otherProfileId];
            var isFirstRequested = chat.FirstProfileId == profileId;
            
            string? isBlockedMessage = null;

            if (blockerBlackList.Any(bl => bl.BlockedProfileId == otherProfileId))
            {
                isBlockedMessage = "Вы заблокировали пользователя";
            }
            else if (blockedBlackList.Any(bl => bl.BlockerProfileId == otherProfileId))
            {
                isBlockedMessage = "Вас заблокировали";
            }
            
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
                IsBlockedMessage = isBlockedMessage,
            };
        }).ToList();

        return new PagedList<ChatResponseDto>(chatResponseDtos, count, pageNumber, pageSize);
    }
    
    public async Task<ChatResponseDto> CreateChatAsync(long firstProfileId, long secondProfileId, CancellationToken cancellationToken)
    {
        var profile1 = await _unitOfWork.Profiles.GetByIdAsync(firstProfileId, cancellationToken);

        if (profile1 is null)
        {
            throw new NotFoundException("Профиль");
        }
        
        var profile2 = await _unitOfWork.Profiles.GetByIdAsync(secondProfileId, cancellationToken);

        if (profile2 is null)
        {
            throw new NotFoundException("Профиль");
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
            throw new NotFoundException("Чат");
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
            throw new NotFoundException("Чат");
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
            throw new NotFoundException("Чат");
        }

        return chat;
    }

    public async Task<ChatResponseDto> IncrementUnreadCountAsync(string chatId, long receiverId)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, CancellationToken.None);

        if (chat is null)
        {
            throw new NotFoundException("Чат");
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
        };
    }
    
    private async Task<ChatResponseDto> MapToChatResponseDtoAsync(Chat chat, long currentProfileId, CancellationToken cancellationToken)
    {
        var otherProfileId = chat.FirstProfileId == currentProfileId ? chat.SecondProfileId : chat.FirstProfileId;

        var profiles = await _unitOfWork.Profiles.GetAsync(
            p => p.Id == currentProfileId || p.Id == otherProfileId,
            cancellationToken
        );

        var profileDict = profiles.ToDictionary(p => p.Id);

        var blockerBlackList = await _unitOfWork.BlackLists.GetByBlockerIdAsync(currentProfileId, cancellationToken);
        var blockedBlackList = await _unitOfWork.BlackLists.GetByBlockedIdAsync(currentProfileId, cancellationToken);

        var isFirstRequested = chat.FirstProfileId == currentProfileId;
        var otherProfile = profileDict[otherProfileId];

        string? isBlockedMessage = null;
        if (blockerBlackList.Any(bl => bl.BlockedProfileId == otherProfileId))
        {
            isBlockedMessage = "Вы заблокировали пользователя";
        }
        else if (blockedBlackList.Any(bl => bl.BlockerProfileId == otherProfileId))
        {
            isBlockedMessage = "Вас заблокировали";
        }

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
            IsBlockedMessage = isBlockedMessage,
        };
    }
}