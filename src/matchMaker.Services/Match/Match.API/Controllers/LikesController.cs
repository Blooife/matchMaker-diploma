using Common.Constants;
using Match.BusinessLogic.DTOs.Like;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Match.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.User}")]
public class LikesController(ILikeService _likeService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<LikeResponseDto>> AddLike([FromBody]AddLikeDto dto, CancellationToken cancellationToken)
    {
        var like = await _likeService.AddLikeAsync(dto, cancellationToken);

        return Ok(like);
    }
}