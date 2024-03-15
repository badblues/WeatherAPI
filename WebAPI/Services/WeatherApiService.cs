using System.Net;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Services;

public class WeatherApiService
{
    //Could place in configs, but it's not likely to change
    private readonly string apiURL = "https://api.openweathermap.org/data/3.0/onecall";

    private readonly HttpClient _httpClient;

    private readonly string _defaultApiKey;

    public WeatherApiService(HttpClient httpClient, string defaultApiKey)
    {
        _httpClient = httpClient;
        _defaultApiKey = defaultApiKey;
    }

    public async Task<WeatherTimeZoneInfo> GetWeatherAsync(double lat, double lon, string? apiKey)
    {
        string appid = apiKey ?? _defaultApiKey;

        string response = await _httpClient.
            GetStringAsync($"{apiURL}?exclude=minutely,hourly,daily,alerts&lat={lat}&lon={lon}&appid={appid}");

        WeatherTimeZoneInfo? weatherInfo = JsonConvert.DeserializeObject<WeatherTimeZoneInfo>(response);

        if (weatherInfo == null || weatherInfo.Weather == null)
            throw new HttpRequestException("Couldn't get weather info", null, HttpStatusCode.NotFound);

        return weatherInfo;
    }
}
