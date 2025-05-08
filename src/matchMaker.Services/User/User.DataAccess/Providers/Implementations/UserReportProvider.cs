using Common.Enums;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using User.DataAccess.Contexts;
using User.DataAccess.Dtos;
using User.DataAccess.Models;
using User.DataAccess.Providers.Interfaces;

namespace User.DataAccess.Providers.Implementations;

public class UserReportProvider(UserContext _dbContext) : IUserReportProvider
{
    public async Task<bool> HasUserAlreadyReportedAsync(long reporterId, long reportedId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserReports.AnyAsync(
            r => r.ReporterUserId == reporterId && r.ReportedUserId == reportedId && r.Status == ReportStatus.Pending,
            cancellationToken);
    }

    public async Task<UserReport?> GetReportByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserReports
            .Include(r => r.ReportType)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<UserReport>> GetReportsAgainstUserAsync(long reportedUserId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserReports
            .Where(r => r.ReportedUserId == reportedUserId)
            .Include(r => r.Reporter)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateReportAsync(UserReport report, CancellationToken cancellationToken = default)
    {
        _dbContext.UserReports.Add(report);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteReportAsync(long reportId, CancellationToken cancellationToken = default)
    {
        var report = await _dbContext.UserReports.FindAsync(new object?[] { reportId }, cancellationToken);
        if (report != null)
        {
            _dbContext.UserReports.Remove(report);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<ReportType>> GetAllReportTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ReportTypes.AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<ReportType?> GetReportTypeByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ReportTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<(IEnumerable<UserReport> reports, int totalCount)> GetReportsAsync(
        ReportFilterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.UserReports
            .Include(r => r.Reporter)
            .Include(r => r.Reported)
            .Include(r => r.ReportType)
            .Include(r => r.Moderator)
            .AsQueryable();

        if (filter.Status.HasValue)
        {
            query = query.Where(r => r.Status == filter.Status.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.ReporterUserEmail))
        {
            query = query.Where(r => r.Reporter.Email.Contains(filter.ReporterUserEmail));
        }

        if (!string.IsNullOrWhiteSpace(filter.ReportedUserEmail))
        {
            query = query.Where(r => r.Reported.Email.Contains(filter.ReportedUserEmail));
        }

        if (filter.ReportTypeId.HasValue)
        {
            query = query.Where(r => r.ReportTypeId == filter.ReportTypeId.Value);
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(r => r.CreatedAt >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(r => r.CreatedAt <= filter.CreatedTo.Value);
        }

        if (filter.NotReviewed.HasValue)
        {
            if(filter.NotReviewed.Value)
            {
                query = query.Where(r => r.ReviewedByModeratorId == null);
            }
            else
            {
                query = query.Where(r => r.ReviewedByModeratorId != null);
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var reports = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (reports, totalCount);
    }

    public async Task UpdateReportStatusAsync(long reportId, ReportStatus status, string? moderatorComment, long moderatorUserId, CancellationToken cancellationToken = default)
    {
        var report = await _dbContext.UserReports.FindAsync(reportId);
        if (report == null)
        {
            throw new NotFoundException("Жалоба", 2);
        }

        report.Status = status;
        report.ModeratorComment = moderatorComment;
        report.ReviewedByModeratorId = moderatorUserId;
        report.ReviewedAt = DateTime.UtcNow;

        _dbContext.UserReports.Update(report);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}