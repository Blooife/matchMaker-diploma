using Common.Constants;
using Match.BusinessLogic.DTOs.Notification;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Match.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.User}")]
public class NotificationsController(INotificationService _notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<NotificationResponseDto>>> GetNotificationsByProfileId(CancellationToken cancellationToken)
    {
        var profiles = await _notificationService.GetUserNotificationsAsync(cancellationToken);

        return Ok(profiles);
    }
    
    [HttpPost("read")]
    public async Task<ActionResult> MarkNotificationsAsRead(
        [FromBody] MarkAsReadDto dto, CancellationToken cancellationToken)
    {
        await _notificationService.MarkNotificationsAsReadAsync(dto.NotificationIds, cancellationToken);

        return NoContent();
    }
}