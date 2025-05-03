using Common.Enums;
using Common.Models;
using User.BusinessLogic.DTOs.Request;
using User.BusinessLogic.DTOs.Response;
using User.DataAccess.Dtos;

namespace User.BusinessLogic.Services.Interfaces;

public interface IUserReportService
{
    Task<bool> HasUserAlreadyReportedAsync(long reporterId, long reportedId, CancellationToken cancellationToken = default);
    Task CreateReportAsync(CreateUserReportDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserReportDto>> GetReportsAgainstUserAsync(long reportedUserId, CancellationToken cancellationToken = default);
    Task DeleteReportAsync(long reportId, CancellationToken cancellationToken = default);
    Task<PagedList<UserReportDto>> GetReportsAsync(
        ReportFilterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task ModerateReportAsync(ModerateReportDto dto, CancellationToken cancellationToken = default);
    Task<List<ReportTypeDto>> GetReportTypesAsync(CancellationToken cancellationToken = default);
}