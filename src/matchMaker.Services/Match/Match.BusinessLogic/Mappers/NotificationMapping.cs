using Match.BusinessLogic.DTOs.Notification;
using Match.DataAccess.Models;

namespace Match.BusinessLogic.Mappers;

public class NotificationMapping : AutoMapper.Profile
{
    public NotificationMapping()
    {
        CreateMap<Notification, NotificationResponseDto>().ReverseMap();
    }
}