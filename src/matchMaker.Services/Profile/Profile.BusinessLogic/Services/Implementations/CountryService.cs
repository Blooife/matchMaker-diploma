using AutoMapper;
using Common.Exceptions;
using Profile.BusinessLogic.DTOs.City.Response;
using Profile.BusinessLogic.DTOs.Country.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Interfaces;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.BusinessLogic.Services.Implementations;

public class CountryService(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : ICountryService
{
    private readonly string _cacheKeyPrefix = "countries";
    
    public async Task<IEnumerable<CountryResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<CountryResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var countries = await _unitOfWork.CountryRepository.GetAllAsync(cancellationToken);
        
        var mappedCountries = _mapper.Map<List<CountryResponseDto>>(countries);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedCountries, cancellationToken:cancellationToken);
        
        return mappedCountries;
    }
    
    public async Task<IEnumerable<CityResponseDto>> GetAllCitiesByCountryId(long countryId, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{countryId}:cities";
        var cachedData = await _cacheService.GetAsync<IEnumerable<CityResponseDto>>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }

        var country = await _unitOfWork.CountryRepository.FirstOrDefaultAsync(countryId, cancellationToken);

        if (country is null)
        {
            throw new NotFoundException("Страна", 2);    
        }
        
        var cities = await _unitOfWork.CountryRepository.GetAllCitiesFromCountryAsync(countryId, cancellationToken);
        
        var mappedCities = _mapper.Map<List<CityResponseDto>>(cities);
        await _cacheService.SetAsync(cacheKey, mappedCities, cancellationToken:cancellationToken);
        
        return mappedCities;
    }
}