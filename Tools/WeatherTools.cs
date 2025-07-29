using ModelContextProtocol.Server;
using System.ComponentModel;
using WeatherMcpServer.Services;

namespace WeatherMcpServer.Tools
{
    public class WeatherTools
    {
        private readonly IWeatherService _weatherService;

        public WeatherTools(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [McpServerTool]
        [Description("Получает текущие погодные условия для указанного города.")]
        public async Task<string> GetCurrentWeather(
        [Description("Название города, для которого нужно узнать погоду")] string city,
        [Description("Необязательно: код страны (например, 'US', 'GB')")] string? countryCode = null)
        {
            var weather = await _weatherService.GetCurrentWeatherAsync(city, countryCode);

            if (weather == null)
            {
                return $"Не удалось получить данные о погоде для {city}";
            }

            return $"""
            Текущая погода в {weather.Name}:
            Температура: {weather.Main.Temp}°C (ощущается как {weather.Main.FeelsLike}°C)
            Погодные условия: {weather.Weather[0].Description}
            Влажность: {weather.Main.Humidity}%
            Давление: {weather.Main.Pressure} hPa
            Ветер: {weather.Wind.Speed} м/с, направление {weather.Wind.Deg}°
            Облачность: {weather.Clouds.All}%
            Видимость: {weather.Visibility / 1000} км
            """;
        }

        [McpServerTool]
        [Description("Получает прогноз погоды на 3 дня для указанного города.")]
        public async Task<string> GetWeatherForecast(
        [Description("Название города, для которого нужен прогноз")] string city,
        [Description("Необязательно: код страны (например, 'US', 'GB')")] string? countryCode = null)
        {
            var forecast = await _weatherService.GetWeatherForecastAsync(city, countryCode);

            if (forecast == null || forecast.List == null || forecast.List.Count == 0)
                return $"Не удалось получить прогноз погоды для {city}";

            var result = $"Прогноз погоды в {forecast.City.Name} на 3 дня:\n";

            var dailyForecasts = forecast.List
                .GroupBy(f => DateTimeOffset.FromUnixTimeSeconds(f.Dt).Date)
                .Take(3);

            foreach (var day in dailyForecasts)
            {
                var date = day.Key.ToString("dd.MM.yyyy");
                var dayTemp = day.Average(f => f.Main.Temp);
                var nightTemp = day.Where(f => DateTimeOffset.FromUnixTimeSeconds(f.Dt).Hour < 6 || DateTimeOffset.FromUnixTimeSeconds(f.Dt).Hour > 21)
                                  .Average(f => f.Main.Temp);
                var mainWeather = day.GroupBy(f => f.Weather[0].Main)
                                   .OrderByDescending(g => g.Count())
                                   .First().Key;

                result += $"""
                {date}:
                Дневная температура: {dayTemp:F1}°C
                Ночная температура: {nightTemp:F1}°C
                Преобладающие условия: {mainWeather}
                
                """;
            }

            return result;
        }

        [McpServerTool]
        [Description("Получает оповещения о погоде для указанного города.")]
        public async Task<string> GetWeatherAlerts(
        [Description("Название города, для которого нужны предупреждения")] string city,
        [Description("Необязательно: код страны (например, 'US', 'GB')")] string? countryCode = null)
        {
            var alerts = await _weatherService.GetWeatherAlertsAsync(city, countryCode);
            return alerts ?? "Нет данных о предупреждениях";
        }
    }
}
