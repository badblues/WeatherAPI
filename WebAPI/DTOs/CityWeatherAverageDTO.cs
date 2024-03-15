namespace WebAPI.DTOs;

public record CityWeatherAverageDTO
{

    public CityWeatherAverageDTO() { }

    public CityWeatherAverageDTO(CityWeatherDTO firstCity, CityWeatherDTO secondCity)
    {
        FirstCity = firstCity;
        SecondCity = secondCity;
        AverageTemperature = (firstCity.Temperature + secondCity.Temperature) / 2;
        AveragePressure = (firstCity.Pressure + secondCity.Pressure) / 2;
        AverageHumidity = (firstCity.Humidity + secondCity.Humidity) / 2;
        AverageWindSpeed = (firstCity.WindSpeed + secondCity.WindSpeed) / 2;
        AverageOvercast = (firstCity.Overcast + secondCity.Overcast) / 2;
    }

    public CityWeatherDTO? FirstCity { get; set; }

    public CityWeatherDTO? SecondCity { get; set; }

    public double? AverageTemperature { get; set; }

    public double? AveragePressure { get; set; }

    public double? AverageHumidity { get; set; }

    public double? AverageWindSpeed { get; set; }

    public double? AverageOvercast { get; set; }
}
