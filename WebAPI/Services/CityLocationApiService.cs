using System.Net;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Services;

public class CityLocationApiService
{
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
        try
        {
            var json = await _httpClient.GetStringAsync($"{apiURL}?q={city}&appid={apiKey}");

            if (json == null)
            {
                throw new HttpRequestException("City not found", null, HttpStatusCode.NotFound);
            }

            dynamic data = JsonConvert.DeserializeObject(json);

            var cityData = data.First;

            CityLocation cityLocation = new CityLocation
            {
                City = cityData.name,
                Latitude = cityData.lat,
                Longitude = cityData.lon,
            };

            return cityLocation;
        } catch (HttpRequestException)
        {
            throw new HttpRequestException("Unathorized or invalid token", null, HttpStatusCode.Unauthorized);
        }
    }
}
