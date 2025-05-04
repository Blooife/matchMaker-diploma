using Common.Constants;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using User.BusinessLogic.DTOs.Request;
using User.BusinessLogic.DTOs.Response;
using User.BusinessLogic.Services.Interfaces;
using User.DataAccess.Dtos;
using User.DataAccess.Models;

namespace User.API.Controllers;

[Route("api/reports")]
[ApiController]
public class UserReportsController(IUserReportService _userReportService) : ControllerBase
{
    [HttpPost("paginated")]
    [Authorize(Roles = $"{Roles.Moderator}")]
    public async Task<ActionResult<PagedList<UserReportDto>>> GetReports(
        [FromQuery] int pageSize, [FromQuery] int pageNumber,[FromBody] ReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        var pagedList = await _userReportService.GetReportsAsync(filter, pageNumber, pageSize, cancellationToken);
        var metadata = new PaginationMetadata()
        {
            TotalCount = pagedList.TotalCount,
            CurrentPage = pageNumber,
            HasPrevious = pagedList.HasPrevious,
            HasNext = pagedList.HasNext,
            PageSize = pageSize,
            TotalPages = pagedList.TotalPages,
        };

        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        
        return Ok(pagedList);
    }
    
    [HttpPost]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<ActionResult> CreateReport([FromBody] CreateUserReportDto createReportDto, CancellationToken cancellationToken = default)
    {
        await _userReportService.CreateReportAsync(createReportDto, cancellationToken);
        return NoContent();
    }
    
    [HttpPost("moderate")]
    [Authorize(Roles = $"{Roles.Moderator}")]
    public async Task<IActionResult> UpdateReportStatus([FromBody] ModerateReportDto dto, CancellationToken cancellationToken = default)
    {
        await _userReportService.ModerateReportAsync(dto, cancellationToken);
        return NoContent();
    }
    
    [HttpGet("types")]
    [Authorize(Roles = $"{Roles.User}, {Roles.Moderator}")]
    public async Task<ActionResult<List<ReportType>>> GetReportTypes(CancellationToken cancellationToken = default)
    {
        var reportTypes = await _userReportService.GetReportTypesAsync(cancellationToken);
        
        return Ok(reportTypes);
    }
}