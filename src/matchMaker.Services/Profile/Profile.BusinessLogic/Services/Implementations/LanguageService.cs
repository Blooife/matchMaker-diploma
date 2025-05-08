using AutoMapper;
using Common.Exceptions;
using Profile.BusinessLogic.DTOs.Language.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using ILanguageService = Profile.BusinessLogic.Services.Interfaces.ILanguageService;

namespace Profile.BusinessLogic.Services.Implementations;

public class LanguageService(IUnitOfWork _unitOfWork, ICacheService _cacheService, IMapper _mapper) : ILanguageService
{
    private readonly string _cacheKeyPrefix = "languages";
    
    public async Task<IEnumerable<LanguageResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<LanguageResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var languages = await _unitOfWork.LanguageRepository.GetAllAsync(cancellationToken);
        
        var mappedLanguages = _mapper.Map<List<LanguageResponseDto>>(languages);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedLanguages, cancellationToken:cancellationToken);
        
        return mappedLanguages;
    }
    
    public async Task<LanguageResponseDto> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{id}";
        var cachedData = await _cacheService.GetAsync<LanguageResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(id, cancellationToken);
        
        if (language == null)
        {
            throw new NotFoundException("Язык");
        }
        
        var mappedLanguage = _mapper.Map<LanguageResponseDto>(language);
        await _cacheService.SetAsync(cacheKey, mappedLanguage, cancellationToken:cancellationToken);
        
        return mappedLanguage;
    }
}