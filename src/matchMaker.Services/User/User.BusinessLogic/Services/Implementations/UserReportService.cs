using Common.Authorization.Context;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using MessageQueue;
using MessageQueue.Messages.User;
using User.BusinessLogic.DTOs.Request;
using User.BusinessLogic.DTOs.Response;
using User.BusinessLogic.Services.Interfaces;
using User.DataAccess.Dtos;
using User.DataAccess.Models;
using User.DataAccess.Providers.Interfaces;

namespace User.BusinessLogic.Services.Implementations;

public class UserReportService(
    IUserReportProvider _userReportProvider,
    IAuthenticationContext _authenticationContext,
    IUserProvider _userProvider,
    ICommunicationBus _communicationBus) : IUserReportService
{
    public async Task<bool> HasUserAlreadyReportedAsync(long reporterId, long reportedId, CancellationToken cancellationToken = default)
    {
        return await _userReportProvider.HasUserAlreadyReportedAsync(reporterId, reportedId, cancellationToken);
    }

    public async Task CreateReportAsync(CreateUserReportDto dto, CancellationToken cancellationToken = default)
    {
        var alreadyReported = await _userReportProvider.HasUserAlreadyReportedAsync(_authenticationContext.UserId, dto.ReportedUserId, cancellationToken);
        if (alreadyReported)
        {
            throw new InvalidOperationException("Вы уже подали жалобу на этого пользователя.");
        }
        
        var reportType = await _userReportProvider.GetReportTypeByIdAsync(dto.ReportTypeId, cancellationToken);
        if (reportType is null)
        {
            throw new NotFoundException(dto.ReportTypeId);
        }

        var report = new UserReport
        {
            ReporterUserId = _authenticationContext.UserId,
            ReportedUserId = dto.ReportedUserId,
            Reason = dto.Reason,
            ReportTypeId = dto.ReportTypeId,
            CreatedAt = DateTime.UtcNow
        };

        await _userReportProvider.CreateReportAsync(report, cancellationToken);
    }

    public async Task<IEnumerable<UserReportDto>> GetReportsAgainstUserAsync(long reportedUserId, CancellationToken cancellationToken = default)
    {
        var reports = await _userReportProvider.GetReportsAgainstUserAsync(reportedUserId, cancellationToken);

        return reports.Select(r => new UserReportDto
        {
            Id = r.Id,
            ReporterUserId = r.ReporterUserId,
            ReportedUserId = r.ReportedUserId,
            Reason = r.Reason,
            CreatedAt = r.CreatedAt
        });
    }

    public async Task DeleteReportAsync(long reportId, CancellationToken cancellationToken = default)
    {
        await _userReportProvider.DeleteReportAsync(reportId, cancellationToken);
    }
    
    public async Task<PagedList<UserReportDto>> GetReportsAsync(
        ReportFilterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var (reports, count) = await _userReportProvider.GetReportsAsync(filter, pageNumber, pageSize, cancellationToken);
        
        var userReportsDtos = reports.Select(r => new UserReportDto
        {
            Id = r.Id,
            ReporterUserId = r.ReporterUserId,
            ReportedUserId = r.ReportedUserId,
            Reason = r.Reason,
            CreatedAt = r.CreatedAt,
            Status = r.Status.GetDescription(),
            ReportTypeId = r.ReportTypeId,
            ReportType = r.ReportType.Name,
            ReportedEmail = r.Reported.Email,
            ReporterEmail = r.Reporter.Email,
            ModeratorComment = r.ModeratorComment,
            ModeratorEmail = r.Moderator?.Email,
            ReviewedByModeratorId = r.ReviewedByModeratorId,
            ReviewedAt = r.ReviewedAt,
        }).ToList();
        
        return new PagedList<UserReportDto>(userReportsDtos, count, pageNumber, pageSize);
    }

    public async Task ModerateReportAsync(ModerateReportDto dto, CancellationToken cancellationToken = default)
    {
        var report = await _userReportProvider.GetReportByIdAsync(dto.ReportId, cancellationToken);
        
        await _userReportProvider.UpdateReportStatusAsync(dto.ReportId, dto.Status, dto.ModeratorComment, _authenticationContext.UserId, cancellationToken);

        if (dto.Status == ReportStatus.Blocked)
        {
            await _userProvider.BanUserAsync(report.ReportedUserId, dto.BanUntil, cancellationToken);
        }

        await PublishNewReportNotification(report, dto);
    }

    public async Task<List<ReportTypeDto>> GetReportTypesAsync(CancellationToken cancellationToken = default)
    {
        var reportTypes = await _userReportProvider.GetAllReportTypesAsync(cancellationToken);

        return reportTypes.Select(rt => new ReportTypeDto
        {
            Id = rt.Id,
            Name = rt.Name,
        }).ToList();
    }

    private async Task PublishNewReportNotification(UserReport report, ModerateReportDto dto)
    {
        string body = dto.Status switch
        {
            ReportStatus.Rejected => "Жалоба была отклонена модератором.",
            ReportStatus.Reviewed => "Модератор рассмотрел жалобу и решил выдать вам предупреждение.",
            ReportStatus.Blocked when dto.BanUntil == null => 
                "Модератор рассмотрел жалобу и принял решение о бессрочной блокировке вашего аккаунта.",
            ReportStatus.Blocked when dto.BanUntil != null => 
                $"Модератор рассмотрел жалобу и временно заблокировал ваш аккаунт до {dto.BanUntil.Value.ToLocalTime():g}.",
            _ => "Жалоба на вас обновлена."
        };
            
        var notification = new NotificationCreatedEventMessage()
        {
            Body = $"На вас была подана жалоба за {report.ReportType.Name}. {body}",
            NotificationType = NotificationType.NewReportOnYou,
            UserId = report.ReportedUserId,
        };
        
        await _communicationBus.PublishAsync(notification);
    }
}