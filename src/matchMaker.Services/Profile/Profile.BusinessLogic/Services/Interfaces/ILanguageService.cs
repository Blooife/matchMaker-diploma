using Profile.BusinessLogic.DTOs.Language.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface ILanguageService
{
    Task<IEnumerable<LanguageResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<LanguageResponseDto> GetByIdAsync(long id, CancellationToken cancellationToken);
}