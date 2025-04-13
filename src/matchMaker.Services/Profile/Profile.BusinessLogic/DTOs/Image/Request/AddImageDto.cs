using Microsoft.AspNetCore.Http;

namespace Profile.BusinessLogic.DTOs.Image.Request;

public class AddImageDto
{
    public long ProfileId { get; set; }
    public IFormFile file { get; set; }
}