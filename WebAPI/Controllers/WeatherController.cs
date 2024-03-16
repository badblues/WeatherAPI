using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("/weather")]
public class WeatherController : ControllerBase
{
    private readonly WeatherDataService _weatherDataService;

    public WeatherController(WeatherDataService weatherDataService)
    {
        _weatherDataService = weatherDataService;
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<CityWeatherDTO>> GetAsync(string city, string? apiKey)
    {
        return await _weatherDataService.GetCityWeatherDTO(city, apiKey);
    }

    [HttpGet("{firstCity}/{secondCity}")]
    public async Task<ActionResult<AverageCityWeatherDTO>> GetAsync(string firstCity, string secondCity, string? apiKey)
    {
        return await _weatherDataService.GetAverageCityWeatherDTO(firstCity, secondCity, apiKey);
    }

    [HttpGet("{city}/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<CityWeatherDTO>> GetXmlAsync(string city, string? apiKey)
    {
        return await GetAsync(city, apiKey);
    }

    [HttpGet("{firstCity}/{secondCity}/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<AverageCityWeatherDTO>> GetXmlAsync(string firstCity, string secondCity, string? apiKey)
    {
        return await GetAsync(firstCity, secondCity, apiKey);
    }
}
