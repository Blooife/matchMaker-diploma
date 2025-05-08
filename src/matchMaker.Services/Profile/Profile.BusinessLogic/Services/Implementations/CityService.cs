using AutoMapper;
using Common.Exceptions;
using Profile.BusinessLogic.DTOs.City.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Interfaces;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.BusinessLogic.Services.Implementations;

public class CityService(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : ICityService
{
    private readonly string _cacheKeyPrefix = "cities";
    
    public async Task<IEnumerable<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<CityResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var cities = await _unitOfWork.CityRepository.GetAllAsync(cancellationToken);
        
        var mappedCities = _mapper.Map<List<CityResponseDto>>(cities);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedCities, cancellationToken:cancellationToken);
        
        return mappedCities;
    }
    
    public async Task<CityResponseDto> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{id}";
        var cachedData = await _cacheService.GetAsync<CityResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var city = await _unitOfWork.CityRepository.FirstOrDefaultAsync(id, cancellationToken);
        
        if (city is null)
        {
            throw new NotFoundException("Город");
        }
        
        var mappedCity = _mapper.Map<CityResponseDto>(city);
        await _cacheService.SetAsync(cacheKey, mappedCity, cancellationToken:cancellationToken);
        
        return mappedCity;
    }
}