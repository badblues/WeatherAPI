using System.Net;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        try
        {
            return await _weatherDataService.GetCityWeatherDTO(city, apiKey);
        }
        catch (HttpRequestException ex)
        {
            return HandleHttpRequestException(ex);
        }
    }

    [HttpGet("{firstCity}/{secondCity}")]
    public async Task<ActionResult<AverageCityWeatherDTO>> GetAsync(string firstCity, string secondCity, string? apiKey)
    {
        try
        {
           return await _weatherDataService.GetCityWeatherAverageDTO(firstCity, secondCity, apiKey);
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
    public async Task<ActionResult<AverageCityWeatherDTO>> GetXmlAsync(string firstCity, string secondCity, string? apiKey)
    {
        return await GetAsync(firstCity, secondCity, apiKey);
    }

    private ObjectResult HandleHttpRequestException(HttpRequestException ex)
    {
        Log.Error("Error occured: @Message", ex.Message);
        return ex.StatusCode switch
        {
            HttpStatusCode.NotFound => NotFound(ex.Message),
            HttpStatusCode.Unauthorized => Unauthorized("Unauthorized or invalid apiKey"),
            //Don't know what else to return if something unexpected happens
            _ => StatusCode((int)HttpStatusCode.InternalServerError, "Unexpected error occured"),
        };
    }


}

