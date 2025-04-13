using Profile.BusinessLogic.DTOs.Goal.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class GoalMapping : AutoMapper.Profile
{
    public GoalMapping()
    {
        CreateMap<Goal, GoalResponseDto>().ReverseMap();
    }
}