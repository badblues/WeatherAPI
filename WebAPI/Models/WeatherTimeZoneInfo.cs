using Newtonsoft.Json;

namespace WebAPI.Models;

public class WeatherTimeZoneInfo
{
    [JsonProperty("timezone_offset")]
    public long TimeOffsetSeconds { get; set; }

    [JsonProperty("current")]
    public WeatherInfo Weather { get; set; }    
}
