namespace WebAPI.DTOs;

public record CityWeatherDTO
{
    public string CityName { get; set; } = "";

    public DateTime? CityTime { get; set; }

    public DateTime ServerTime { get; set; }

    public TimeSpan? CityTimeDifference { get; set; }

    public double? Temperature { get; set; }

    public double? Pressure { get; set; }

    public double? Humidity { get; set; }

    public double? WindSpeed { get; set; }

    public double? Overcast { get; set; }
}
