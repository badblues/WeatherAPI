using System;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using WebAPI.DTOs;

namespace WebAPI.Extensions;

public static class WeatherConverter
{

    public static WeatherDTO JsonToWeatherDTO(string json)
    {
        dynamic data = JsonConvert.DeserializeObject(json);

        long timeS = Convert.ToInt64(data.current.dt);
        long timeOffset = Convert.ToInt64(data.timezone_offset);

        DateTimeOffset cityTime = DateTimeOffset.FromUnixTimeSeconds(timeS);
        cityTime = cityTime.ToOffset(TimeSpan.FromSeconds(timeOffset));

        double temperature = KelvinToCelsius(Convert.ToDouble(data.current.temp));

        WeatherDTO weather = new WeatherDTO {
            CityTime = cityTime.DateTime,
            Temperature = temperature,
            Pressure = data.current.pressure,
            Humidity = data.current.humidity,
            WindSpeed = data.current.wind_speed,
            Overcast = data.current.clouds,
        };

        return weather;
    }

    public static double KelvinToCelsius(double tempKelvin)
    {
        return tempKelvin - 273.15;
    }

}
