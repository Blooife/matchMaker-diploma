using AutoMapper;
using Common.Constants;
using Common.Models;
using Match.BusinessLogic.DTOs.Chat;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Match.API.Controllers;

[ApiController]
[Route("api/chats")]
[Authorize(Roles = $"{Roles.User}")]
public class ChatsController(IChatService _chatService, IMapper _mapper) : ControllerBase
{
    [HttpGet("profiles")]
    public async Task<ActionResult<ICollection<ChatResponseDto>>> GetByProfilesIds(
        CancellationToken cancellationToken, [FromQuery] long firstProfileId, [FromQuery] long secondProfileId)
    {
        var chat = await _chatService.GetChatsByProfileIdsAsync(firstProfileId, secondProfileId, cancellationToken);

        return Ok(chat);
    }
    
    [HttpGet("paged")]
    public async Task<ActionResult<PagedList<ChatResponseDto>>> GetPagedChats(
        CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pagedList = await _chatService.GetChatsByProfileId(pageNumber, pageSize, cancellationToken);
        var metadata = _mapper.Map<PaginationMetadata>(pagedList);
        
        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        return Ok(pagedList);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateChat([FromBody] CreateChatDto dto, CancellationToken cancellationToken)
    {
        var chat = await _chatService.CreateChatAsync(dto.FirstProfileId, dto.SecondProfileId, cancellationToken);

        return Ok(chat);
    }
    
    [HttpDelete("{chatId}")]
    public async Task<ActionResult<GeneralResponseDto>> DeleteChat([FromRoute] string chatId, CancellationToken cancellationToken)
    {
        var chat = await _chatService.DeleteChatAsync(chatId, cancellationToken);

        return Ok(chat);
    }
    
    [HttpPost("read")]
    public async Task<ActionResult> ReadChat([FromBody] ReadChatDto dto, CancellationToken cancellationToken)
    {
        await _chatService.ReadChatAsync(dto, cancellationToken);

        return Ok();
    }
}