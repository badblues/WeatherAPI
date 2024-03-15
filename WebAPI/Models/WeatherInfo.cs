using Newtonsoft.Json;

namespace WebAPI.Models;

public class WeatherInfo
{
    [JsonProperty("dt")]
    public long TimeSeconds { get; set; }

    [JsonProperty("temp")]
    public double Temperature { get; set; }

    [JsonProperty("pressure")]
    public double Pressure { get; set; }

    [JsonProperty("humidity")]
    public double Humidity { get; set; }

    [JsonProperty("wind_speed")]
    public double WindSpeed { get; set; }

    [JsonProperty("clouds")]
    public double Overcast { get; set; }
}