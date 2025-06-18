using AutoMapper;
using Common.Constants;
using Common.Models;
using Match.BusinessLogic.DTOs.Match;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Match.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.User}")]
public class MatchesController(IMatchService _matchService, IMapper _mapper, ICompatibilityService _compatibilityService) : ControllerBase
{
    [HttpGet("paged")]
    public async Task<ActionResult<PagedList<MatchResponseDto>>> GetPagedMatches(
        CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pagedList = await _matchService.GetMatchesByProfileIdAsync(pageNumber, pageSize, cancellationToken);
        var metadata = _mapper.Map<PaginationMetadata>(pagedList);

        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        return Ok(pagedList);
    }
    
    [HttpPost("compatibility")]
    public async Task<IActionResult> GetCompatibility([FromBody] CompatibilityRequestDto request)
    {
        var compatibilityJson = await _compatibilityService.GetBirthCompatibilityAsync(request.BirthDate1, request.BirthDate2);

        return Content(compatibilityJson, "application/json");
    }
}