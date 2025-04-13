using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.City.Response;
using Profile.BusinessLogic.DTOs.Country.Response;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class CountriesController(ICountryService _countryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<CountryResponseDto>>> GetAllCountries(CancellationToken cancellationToken)
    {
        var countries = await _countryService.GetAllAsync(cancellationToken);
        
        return Ok(countries);
    }
    
    [HttpGet("{id}/cities")]
    public async Task<ActionResult<ICollection<CityResponseDto>>> GetCitiesFromCountry([FromRoute] int id, CancellationToken cancellationToken)
    {
        var cities = await _countryService.GetAllCitiesByCountryId(id, cancellationToken);
        
        return Ok(cities);
    }
}