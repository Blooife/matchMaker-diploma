using Match.BusinessLogic.DTOs.Message;
using Match.DataAccess.Models;

namespace Match.BusinessLogic.Mappers;

public class MessageMapping : AutoMapper.Profile
{
    public MessageMapping()
    {
        CreateMap<Message, MessageResponseDto>()
            .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id))
            .ForMember(dest=>dest.Content, opt=>opt.MapFrom(src=>src.Content))
            .ForMember(dest=>dest.SenderId, opt=>opt.MapFrom(src=>src.SenderId))
            .ForMember(dest=>dest.ChatId, opt=>opt.MapFrom(src=>src.ChatId))
            .ForMember(dest=>dest.Timestamp, opt=>opt.MapFrom(src=>src.Timestamp));
    }
}