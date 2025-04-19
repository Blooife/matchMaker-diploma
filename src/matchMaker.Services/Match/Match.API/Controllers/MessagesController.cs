using AutoMapper;
using Common.Constants;
using Common.Models;
using Match.BusinessLogic.DTOs.Message;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Match.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.User}")]
public class MessagesController(IMessageService _messageService, IMapper _mapper) : ControllerBase
{
    [HttpGet("paged/{chatId}")]
    public async Task<ActionResult<PagedList<MessageResponseDto>>> GetPagedMessages(
        [FromRoute] string chatId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pagedList = await _messageService.GetMessagesByChatIdAsync(chatId, pageNumber, pageSize, cancellationToken);
        var metadata = _mapper.Map<PaginationMetadata>(pagedList);
        
        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        return Ok(pagedList);
    }
}