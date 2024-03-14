namespace WebAPI.Services;

public class WeatherInfoApiService
{
    private string apiURL = "https://api.openweathermap.org/data/3.0/onecall";

    private HttpClient _httpClient;

    public WeatherInfoApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetWeather(double lat, double lon, string apiKey)
    {
        var json = await _httpClient.GetStringAsync($"{apiURL}?lat={lat}&lon={lon}&appid={apiKey}");
        return json;
    }

}
