using Profile.BusinessLogic.DTOs.User.Request;
using Profile.BusinessLogic.DTOs.User.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class UserMapping : AutoMapper.Profile
{
    public UserMapping()
    {
        CreateMap<DataAccess.Models.User, UserResponseDto>();
        CreateMap<CreateOrUpdateUserDto, DataAccess.Models.User>();
    }
}