using System.Net;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Services;

public class CityLocationApiService
{
    //Could place in configs, but it's not likely to change
    private readonly string apiURL = "https://api.openweathermap.org/geo/1.0/direct";

    private readonly HttpClient _httpClient;

    private readonly string _defaultApiKey;

    public CityLocationApiService(HttpClient httpClient, string defaultApiKey)
    {
        _httpClient = httpClient;
        _defaultApiKey = defaultApiKey;
    }

    public async Task<CityLocation> GetLocationAsync(string city, string? apiKey)
    {
        string appid = apiKey ?? _defaultApiKey;

        string response = await _httpClient.GetStringAsync($"{apiURL}?q={city}&appid={appid}");

        IList<CityLocation>? cities = JsonConvert.DeserializeObject<IList<CityLocation>>(response);

        if (cities == null || cities.Count == 0)
            throw new HttpRequestException($"Not found city: {city}", null, HttpStatusCode.NotFound);

        return cities[0];
    }
}
