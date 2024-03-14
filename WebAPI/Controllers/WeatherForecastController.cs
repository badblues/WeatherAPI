using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebAPI.DTOs;
using WebAPI.Extensions;
using WebAPI.Models;
using System.Net;

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
    public async Task<ActionResult<WeatherDTO>> Get(string city, string? apiKey = null)
    {
        try
        {
            CityLocation location = apiKey != null ?
                await _cityLocationApiService.GetLocation(city, apiKey) :
                await _cityLocationApiService.GetLocation(city);

            var weatherJson = apiKey != null ?
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude, apiKey) :
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude);

            WeatherDTO weatherDTO = ConvertJsonToWeatherDTO(weatherJson, location.City);
            return weatherDTO;
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound(ex.Message);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized(ex.Message);
                default:
                    return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("{firstCity}/{secondCity}")]
    public async Task<ActionResult<WeatherDifferenceDTO>> Get(string firstCity, string secondCity, string? apiKey = null)
    {
        try
        {
            CityLocation firstLocation = apiKey != null ?
                await _cityLocationApiService.GetLocation(firstCity, apiKey) :
                await _cityLocationApiService.GetLocation(firstCity);

            CityLocation secondLocation = apiKey != null ?
                await _cityLocationApiService.GetLocation(secondCity, apiKey) :
                await _cityLocationApiService.GetLocation(secondCity);

            var firstWeatherJson = apiKey != null ?
                await _openWeatherMapApi.GetWeather(firstLocation.Latitude, firstLocation.Longitude, apiKey) :
                await _openWeatherMapApi.GetWeather(firstLocation.Latitude, firstLocation.Longitude);

            var secondWeatherJson = apiKey != null ?
               await _openWeatherMapApi.GetWeather(secondLocation.Latitude, secondLocation.Longitude, apiKey) :
               await _openWeatherMapApi.GetWeather(secondLocation.Latitude, secondLocation.Longitude);

            WeatherDTO firstWeatherDTO = ConvertJsonToWeatherDTO(firstWeatherJson, firstLocation.City);
            WeatherDTO secondWeatherDTO = ConvertJsonToWeatherDTO(secondWeatherJson, secondLocation.City);

            return new WeatherDifferenceDTO(firstWeatherDTO, secondWeatherDTO);
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound(ex.Message);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized(ex.Message);
                default:
                    return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("{city}/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<WeatherDTO>> GetXML(string city, string? apiKey = null)
    {
        try
        {
            CityLocation location = apiKey != null ?
                await _cityLocationApiService.GetLocation(city, apiKey) :
                await _cityLocationApiService.GetLocation(city);

            var weatherJson = apiKey != null ?
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude, apiKey) :
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude);

            WeatherDTO weatherDTO = ConvertJsonToWeatherDTO(weatherJson, location.City);
            return weatherDTO;
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound(ex.Message);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized(ex.Message);
                default:
                    return BadRequest(ex.Message);
            }
        }
    }

    private WeatherDTO ConvertJsonToWeatherDTO(string weatherJson, string city)
    {
        WeatherDTO weatherDTO = WeatherConverter.JsonToWeatherDTO(weatherJson);
        weatherDTO.CityName = city;
        weatherDTO.ServerTime = DateTime.Now;
        weatherDTO.CityTimeDifference = weatherDTO.CityTime - weatherDTO.ServerTime;
        return weatherDTO;
    }
}

