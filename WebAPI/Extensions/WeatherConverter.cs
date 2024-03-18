using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Extensions;

public static class WeatherConverter
{

    public static CityWeatherDTO AsDTO(this WeatherTimeZoneInfo weatherInfo)
    {
        DateTime? cityTime = null;
        double? temperature = null;

        if (weatherInfo.Weather != null) {
            if (weatherInfo.Weather.TimeSeconds.HasValue)
            {
                long timeSeconds = weatherInfo.Weather.TimeSeconds.Value;
                TimeSpan offsetTimeSpan = TimeSpan.FromSeconds(weatherInfo.TimeOffsetSeconds);
                cityTime = DateTimeOffset.FromUnixTimeSeconds(timeSeconds)
                    .ToOffset(offsetTimeSpan).DateTime;
            }

            if (weatherInfo.Weather.Temperature.HasValue)
                temperature = KelvinToCelsius(weatherInfo.Weather.Temperature.Value);
        }

        return new CityWeatherDTO
        {
            CityTime = cityTime,
            Temperature = temperature,
            Pressure = weatherInfo.Weather?.Pressure,
            Humidity = weatherInfo.Weather?.Humidity,
            WindSpeed = weatherInfo.Weather?.WindSpeed,
            Overcast = weatherInfo.Weather?.Overcast
        };
    }

    public static double KelvinToCelsius(double tempKelvin)
    {
        return tempKelvin - 273.15;
    }
}
