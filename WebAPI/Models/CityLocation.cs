using Newtonsoft.Json;

namespace WebAPI.Models;

public class CityLocation
{
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("lat")]
    public double Latitude { get; set; }

    [JsonProperty("lon")]
    public double Longitude { get; set; }
}
