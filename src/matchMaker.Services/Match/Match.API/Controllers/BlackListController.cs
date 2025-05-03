using Common.Constants;
using Match.BusinessLogic.DTOs.BlackList;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Match.API.Controllers;

[ApiController]
[Route("api/black-lists")]
[Authorize(Roles = $"{Roles.User}")]
public class BlackListController(IBlackListService blackListService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddToBlackList([FromBody] CreateBlackListDto dto, CancellationToken cancellationToken)
    {
        await blackListService.AddToBlackListAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<BlackListResponseDto>>> GetBlackList(CancellationToken cancellationToken)
    {
        var result = await blackListService.GetBlackListForUserAsync(cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveFromBlackList([FromBody] RemoveFromBlackListDto dto, CancellationToken cancellationToken)
    {
        await blackListService.RemoveFromBlackListAsync(dto, cancellationToken);
        return NoContent();
    }
}