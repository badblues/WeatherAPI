using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Extensions;

public static class WeatherConverter
{

    public static CityWeatherDTO AsDTO(this WeatherTimeZoneInfo weatherInfo)
    {
        DateTimeOffset cityTime = DateTimeOffset.FromUnixTimeSeconds(weatherInfo.Weather.TimeSeconds);
        cityTime = cityTime.ToOffset(TimeSpan.FromSeconds(weatherInfo.TimeOffsetSeconds));

        return new CityWeatherDTO
        {
            CityTime = cityTime.DateTime,
            Temperature = KelvinToCelsius(weatherInfo.Weather.Temperature),
            Pressure = weatherInfo.Weather.Pressure,
            Humidity = weatherInfo.Weather.Humidity,
            WindSpeed = weatherInfo.Weather.WindSpeed,
            Overcast = weatherInfo.Weather.Overcast
        };
    }

    public static double KelvinToCelsius(double tempKelvin)
    {
        return tempKelvin - 273.15;
    }

}
