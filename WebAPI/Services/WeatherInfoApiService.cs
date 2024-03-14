namespace WebAPI.Services;

public class WeatherInfoApiService
{
    private readonly string apiURL = "https://api.openweathermap.org/data/3.0/onecall";

    private readonly HttpClient _httpClient;

    private readonly string _defaultApiKey;

    public WeatherInfoApiService(HttpClient httpClient, string defaultApiKey)
    {
        _httpClient = httpClient;
        _defaultApiKey = defaultApiKey;
    }

    public async Task<string> GetWeather(double lat, double lon)
    {
        return await GetWeather(lat, lon, _defaultApiKey);
    }

    public async Task<string> GetWeather(double lat, double lon, string apiKey)
    {
        try
        {
            var json = await _httpClient.GetStringAsync($"{apiURL}?exclude=minutely,hourly,daily,alerts&lat={lat}&lon={lon}&appid={apiKey}");
            return json;
        }
        catch (HttpRequestException)
        {
            throw;
        }
    }

}
