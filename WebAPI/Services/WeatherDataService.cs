using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Extensions;

namespace WebAPI.Services;

public class WeatherDataService
{
    private readonly WeatherApiService _weatherApiService;
    private readonly CityLocationApiService _cityLocationApiService;

    public WeatherDataService(
        WeatherApiService weatherApiService,
        CityLocationApiService cityLocationApiService)
    {
        _weatherApiService = weatherApiService;
        _cityLocationApiService = cityLocationApiService;
    }

    public async Task<AverageCityWeatherDTO> GetCityWeatherAverageDTO(string firstCity, string secondCity, string? apiKey)
    {
        CityWeatherDTO firstWeatherDTO = await GetCityWeatherDTO(firstCity, apiKey);
        CityWeatherDTO secondWeatherDTO = await GetCityWeatherDTO(secondCity, apiKey);

        return new AverageCityWeatherDTO(firstWeatherDTO, secondWeatherDTO);
    }

    public async Task<CityWeatherDTO> GetCityWeatherDTO(string city, string? apiKey)
    {
        CityLocation location = await _cityLocationApiService.GetLocationAsync(city, apiKey);

        WeatherTimeZoneInfo weatherTimeZoneInfo =
            await _weatherApiService.GetWeatherAsync(location.Latitude, location.Longitude, apiKey);

        return AssembleCityWeatherDTO(weatherTimeZoneInfo, location.Name);
    }

    public CityWeatherDTO AssembleCityWeatherDTO(WeatherTimeZoneInfo weatherInfo, string city)
    {
        CityWeatherDTO weatherDTO = weatherInfo.AsDTO();
        weatherDTO.CityName = city;
        weatherDTO.ServerTime = DateTime.Now;
        weatherDTO.CityTimeDifference = weatherDTO.CityTime - weatherDTO.ServerTime;
        return weatherDTO;
    }
}
