using System.Net;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
    public async Task<ActionResult<CityWeatherDTO>> GetAsync(string city, string? apiKey)
    {
        try
        {
            return await GetCityWeatherDTO(city, apiKey);
        }
        catch (HttpRequestException ex)
        {
            return HandleHttpRequestException(ex);
        }
    }

    [HttpGet("{firstCity}/{secondCity}")]
    public async Task<ActionResult<CityWeatherAverageDTO>> GetAsync(string firstCity, string secondCity, string? apiKey)
    {
        try
        {
            CityWeatherDTO firstWeatherDTO = await GetCityWeatherDTO(firstCity, apiKey);
            CityWeatherDTO secondWeatherDTO = await GetCityWeatherDTO(secondCity, apiKey);

            return new CityWeatherAverageDTO(firstWeatherDTO, secondWeatherDTO);
        }
        catch (HttpRequestException ex)
        {
            return HandleHttpRequestException(ex);
        }
    }

    [HttpGet("{city}/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<CityWeatherDTO>> GetXmlAsync(string city, string? apiKey)
    {
        return await GetAsync(city, apiKey);
    }

    [HttpGet("{firstCity}/{secondCity}/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<CityWeatherAverageDTO>> GetXmlAsync (string firstCity, string secondCity, string? apiKey)
    {
        return await GetAsync(firstCity, secondCity, apiKey);
    }

    private async Task<CityWeatherDTO> GetCityWeatherDTO(string city, string? apiKey)
    {
        CityLocation location = apiKey != null ?
                await _cityLocationApiService.GetLocationAsync(city, apiKey) :
                await _cityLocationApiService.GetLocationAsync(city);

        WeatherTimeZoneInfo weatherTimeZoneInfo = apiKey != null ?
            await _openWeatherMapApi.GetWeatherAsync(location.Latitude, location.Longitude, apiKey) :
            await _openWeatherMapApi.GetWeatherAsync(location.Latitude, location.Longitude);

        return AssembleCityWeatherDTO(weatherTimeZoneInfo, location.Name);
    }

    private ObjectResult HandleHttpRequestException(HttpRequestException ex)
    {
        Log.Error("Error occured: @Message", ex.Message);
        return ex.StatusCode switch
        {
            HttpStatusCode.NotFound => NotFound(ex.Message),
            HttpStatusCode.Unauthorized => Unauthorized("Unauthorized or invalid apiKey"),
            //Don't know what else to return if something unexpected happens
            _ => StatusCode((int)HttpStatusCode.InternalServerError,"Unexpected error occured"),
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

