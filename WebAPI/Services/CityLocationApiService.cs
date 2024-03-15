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

    public async Task<CityLocation> GetLocation(string city)
    {
        return await GetLocation(city, _defaultApiKey);
    }

    public async Task<CityLocation> GetLocation(string city, string apiKey)
    {
        string response = await _httpClient.GetStringAsync($"{apiURL}?q={city}&appid={apiKey}");

        IList<CityLocation>? cities = JsonConvert.DeserializeObject<IList<CityLocation>>(response);

        if (cities == null || cities.Count == 0)
            throw new HttpRequestException("City not found", null, HttpStatusCode.NotFound);

        return cities[0];
    }
}
