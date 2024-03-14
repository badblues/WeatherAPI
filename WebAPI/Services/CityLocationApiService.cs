using Newtonsoft.Json;

namespace WebAPI.Services;

public class CityLocationApiService
{
    private string apiURL = "https://api.openweathermap.org/geo/1.0/direct";

    private HttpClient _httpClient;

    public CityLocationApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<Tuple<double, double>> GetCoordinates(string city, string apiKey)
    {
        var json = await _httpClient.GetStringAsync($"{apiURL}?q={city}&appid={apiKey}");

        dynamic data = JsonConvert.DeserializeObject(json);

        double lat = Convert.ToDouble(data.First.lat);
        double lon = Convert.ToDouble(data.First.lon);

        return Tuple.Create(lat, lon);
    }
}
