using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebAPI.DTOs;
using WebAPI.Extensions;
using WebAPI.Models;
using System.Net;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly WeatherInfoApiService _openWeatherMapApi;
    private readonly CityLocationApiService _cityLocationApiService;

    public WeatherController(
        WeatherInfoApiService openWeatherMapApi,
        CityLocationApiService cityLocationApiService)
    {
        _openWeatherMapApi = openWeatherMapApi;
        _cityLocationApiService = cityLocationApiService;
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<CityWeatherDTO>> Get(string city, string? apiKey = null)
    {
        try
        {
            CityLocation location = apiKey != null ?
                await _cityLocationApiService.GetLocation(city, apiKey) :
                await _cityLocationApiService.GetLocation(city);

            WeatherTimeZoneInfo weatherInfo = apiKey != null ?
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude, apiKey) :
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude);

            CityWeatherDTO weatherDTO = ConvertJsonToWeatherDTO(weatherInfo, location.Name);
            return weatherDTO;
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound(ex.Message);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized("Unauthorized or invalid apiKey");
                default:
                    return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("{firstCity}/{secondCity}")]
    public async Task<ActionResult<CityWeatherDifferenceDTO>> Get(string firstCity, string secondCity, string? apiKey = null)
    {
        try
        {
            CityLocation firstLocation = apiKey != null ?
                await _cityLocationApiService.GetLocation(firstCity, apiKey) :
                await _cityLocationApiService.GetLocation(firstCity);

            CityLocation secondLocation = apiKey != null ?
                await _cityLocationApiService.GetLocation(secondCity, apiKey) :
                await _cityLocationApiService.GetLocation(secondCity);

            WeatherTimeZoneInfo firstWeatherInfo = apiKey != null ?
                await _openWeatherMapApi.GetWeather(firstLocation.Latitude, firstLocation.Longitude, apiKey) :
                await _openWeatherMapApi.GetWeather(firstLocation.Latitude, firstLocation.Longitude);

            WeatherTimeZoneInfo secondWeatherInfo = apiKey != null ?
               await _openWeatherMapApi.GetWeather(secondLocation.Latitude, secondLocation.Longitude, apiKey) :
               await _openWeatherMapApi.GetWeather(secondLocation.Latitude, secondLocation.Longitude);

            CityWeatherDTO firstWeatherDTO = ConvertJsonToWeatherDTO(firstWeatherInfo, firstLocation.Name);
            CityWeatherDTO secondWeatherDTO = ConvertJsonToWeatherDTO(secondWeatherInfo, secondLocation.Name);

            return new CityWeatherDifferenceDTO(firstWeatherDTO, secondWeatherDTO);
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound(ex.Message);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized("Unauthorized or invalid apiKey");
                default:
                    return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("{city}/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<CityWeatherDTO>> GetXML(string city, string? apiKey = null)
    {
        try
        {
            CityLocation location = apiKey != null ?
                await _cityLocationApiService.GetLocation(city, apiKey) :
                await _cityLocationApiService.GetLocation(city);

            WeatherTimeZoneInfo weatherInfo = apiKey != null ?
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude, apiKey) :
                await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude);

            CityWeatherDTO weatherDTO = ConvertJsonToWeatherDTO(weatherInfo, location.Name);
            return weatherDTO;
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return NotFound(ex.Message);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized("Unauthorized or invalid apiKey");
                default:
                    return BadRequest(ex.Message);
            }
        }
    }

    private CityWeatherDTO ConvertJsonToWeatherDTO(WeatherTimeZoneInfo weatherInfo, string city)
    {
        CityWeatherDTO weatherDTO = weatherInfo.AsDTO();
        weatherDTO.CityName = city;
        weatherDTO.ServerTime = DateTime.Now;
        weatherDTO.CityTimeDifference = weatherDTO.CityTime - weatherDTO.ServerTime;
        return weatherDTO;
    }
}

