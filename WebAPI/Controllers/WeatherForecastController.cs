using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly WeatherInfoApiService _openWeatherMapApi;
    private readonly CityLocationApiService _cityLocationApiService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        WeatherInfoApiService openWeatherMapApi,
        CityLocationApiService cityLocationApiService)
    {
        _logger = logger;
        _openWeatherMapApi = openWeatherMapApi;
        _cityLocationApiService = cityLocationApiService;
    }

    [HttpGet("{city}")]
    public async Task<string> Get(string city)
    {
        Tuple<double, double> coords = await _cityLocationApiService.GetCoordinates(city, "f91e0b219e96453d5578560f28786e37");
        var weather = await _openWeatherMapApi.GetWeather(coords.Item1, coords.Item2, "f91e0b219e96453d5578560f28786e37");
        return weather;
    }

    [HttpGet("{city}, {apiKey}")]
    public async Task<string> Get(string city, string apiKey)
    {
        var weather = await _openWeatherMapApi.GetWeather(55.01, 82.55, "f91e0b219e96453d5578560f28786e37");
        return weather;
    }

}

