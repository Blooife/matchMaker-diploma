using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.Image.Request;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class ImagesController(IImageService _imageService) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddImageToProfile(AddImageDto dto, CancellationToken cancellationToken)
    {
        var result = await _imageService.AddImageToProfileAsync(dto, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPatch]
    public async Task<IActionResult> ChangeMainImage(ChangeMainImageDto dto, CancellationToken cancellationToken)
    {
        var result = await _imageService.ChangeMainImageAsync(dto, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> RemoveImageFromProfile([FromBody] RemoveImageDto dto, CancellationToken cancellationToken)
    {
        var result = await _imageService.RemoveImageAsync(dto.ProfileId, dto.ImageId, cancellationToken);
        
        return Ok(result);
    }
}