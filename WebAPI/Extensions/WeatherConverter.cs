using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Extensions;

public static class WeatherConverter
{

    public static CityWeatherDTO AsDTO(this WeatherTimeZoneInfo weatherInfo)
    {
        DateTime? cityTime = null;
        double? temperature = null;
        double? pressure = null;
        double? humidity = null;
        double? windSpeed = null;
        double? overcast = null;

        if (weatherInfo.Weather != null)
        {
            if (weatherInfo.Weather.TimeSeconds.HasValue)
            {
                long timeSeconds = weatherInfo.Weather.TimeSeconds.Value;
                TimeSpan offsetTimeSpan = TimeSpan.FromSeconds(weatherInfo.TimeOffsetSeconds);
                cityTime = DateTimeOffset.FromUnixTimeSeconds(timeSeconds)
                    .ToOffset(offsetTimeSpan).DateTime;
            }

            if (weatherInfo.Weather.Temperature.HasValue)
            {
                temperature = KelvinToCelsius(weatherInfo.Weather.Temperature.Value);
            }

            pressure = weatherInfo.Weather.Pressure;
            humidity = weatherInfo.Weather.Humidity;
            windSpeed = weatherInfo.Weather.WindSpeed;
            overcast = weatherInfo.Weather.Overcast;
        }

        return new CityWeatherDTO
        {
            CityTime = cityTime,
            Temperature = temperature,
            Pressure = pressure,
            Humidity = humidity,
            WindSpeed = windSpeed,
            Overcast = overcast
        };
    }

    public static double KelvinToCelsius(double tempKelvin)
    {
        return tempKelvin - 273.15;
    }
}
