using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.City.Response;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class CitiesController(ICityService _cityService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CityResponseDto>>> GetAllCities(CancellationToken cancellationToken)
    {
        var cities = await _cityService.GetAllAsync(cancellationToken);
        
        return Ok(cities);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CityResponseDto>> GetCityById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var city = await _cityService.GetByIdAsync(id, cancellationToken);
        
        return Ok(city);
    }
}