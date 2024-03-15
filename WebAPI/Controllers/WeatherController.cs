using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Extensions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly WeatherApiService _openWeatherMapApi;
    private readonly CityLocationApiService _cityLocationApiService;

    public WeatherController(
        WeatherApiService openWeatherMapApi,
        CityLocationApiService cityLocationApiService)
    {
        _openWeatherMapApi = openWeatherMapApi;
        _cityLocationApiService = cityLocationApiService;
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<CityWeatherDTO>> Get(string city, string? apiKey)
    {
        try
        {
            return await GetCityWeatherDTO(city, apiKey);
        }
        catch (HttpRequestException ex)
        {
            return GetReturnObject(ex);
        }
    }

    [HttpGet("{firstCity}/{secondCity}")]
    public async Task<ActionResult<CityWeatherAverageDTO>> Get(string firstCity, string secondCity, string? apiKey)
    {
        try
        {
            CityWeatherDTO firstWeatherDTO = await GetCityWeatherDTO(firstCity, apiKey);
            CityWeatherDTO secondWeatherDTO = await GetCityWeatherDTO(secondCity, apiKey);

            return new CityWeatherAverageDTO(firstWeatherDTO, secondWeatherDTO);
        }
        catch (HttpRequestException ex)
        {
            return GetReturnObject(ex);
        }
    }

    [HttpGet("{city}/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<CityWeatherDTO>> GetXML(string city, string? apiKey)
    {
        try
        {
            return await GetCityWeatherDTO(city, apiKey);
        }
        catch (HttpRequestException ex)
        {
            return GetReturnObject(ex);
        }
    }

    private async Task<CityWeatherDTO> GetCityWeatherDTO(string city, string? apiKey)
    {
        CityLocation location = apiKey != null ?
                await _cityLocationApiService.GetLocation(city, apiKey) :
                await _cityLocationApiService.GetLocation(city);

        WeatherTimeZoneInfo weatherTimeZoneInfo = apiKey != null ?
            await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude, apiKey) :
            await _openWeatherMapApi.GetWeather(location.Latitude, location.Longitude);

        return AssembleCityWeatherDTO(weatherTimeZoneInfo, location.Name);
    }

    private ObjectResult GetReturnObject(HttpRequestException ex)
    {
        return ex.StatusCode switch
        {
            HttpStatusCode.NotFound => NotFound(ex.Message),
            HttpStatusCode.Unauthorized => Unauthorized("Unauthorized or invalid apiKey"),
            //Don't know what else to return if something unexpected happens
            _ => BadRequest(ex.Message),
        };
    }

    private CityWeatherDTO AssembleCityWeatherDTO(WeatherTimeZoneInfo weatherInfo, string city)
    {
        CityWeatherDTO weatherDTO = weatherInfo.AsDTO();
        weatherDTO.CityName = city;
        weatherDTO.ServerTime = DateTime.Now;
        weatherDTO.CityTimeDifference = weatherDTO.CityTime - weatherDTO.ServerTime;
        return weatherDTO;
    }
}
