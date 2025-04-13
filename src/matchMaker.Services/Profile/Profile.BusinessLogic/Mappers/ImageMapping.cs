using Profile.BusinessLogic.DTOs.Image.Response;
using Profile.DataAccess.Models;

namespace Profile.BusinessLogic.Mappers;

public class ImageMapping : AutoMapper.Profile
{
    public ImageMapping()
    {
        CreateMap<Image, ImageResponseDto>().ReverseMap();
    }
}