using Common.Enums;
using User.DataAccess.Dtos;
using User.DataAccess.Models;

namespace User.DataAccess.Providers.Interfaces;

public interface IUserReportProvider
{
    Task<bool> HasUserAlreadyReportedAsync(long reporterId, long reportedId, CancellationToken cancellationToken = default);

    Task<UserReport?> GetReportByIdAsync(long id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<UserReport>> GetReportsAgainstUserAsync(long reportedUserId, CancellationToken cancellationToken = default);
    
    Task CreateReportAsync(UserReport report, CancellationToken cancellationToken = default);
    
    Task DeleteReportAsync(long reportId, CancellationToken cancellationToken = default);
    Task<List<ReportType>> GetAllReportTypesAsync(CancellationToken cancellationToken = default);
    Task<ReportType?> GetReportTypeByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<UserReport> reports, int totalCount)> GetReportsAsync(
        ReportFilterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task UpdateReportStatusAsync(long reportId, ReportStatus status, string? moderatorComment, long moderatorUserId, CancellationToken cancellationToken = default);
}