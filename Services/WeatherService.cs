using System.Net.Http.Json;
using WeatherMcpServer.Models;

namespace WeatherMcpServer.Services
{
    internal class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private string? _apiKey;

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public void SetApiKey(string? apiKey) => _apiKey = apiKey;

        public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city, string? countryCode = null)
        {
            try
            {
                var location = BuildLocationQuery(city, countryCode);
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={_apiKey}&units=metric&lang=ru";
                return await _httpClient.GetFromJsonAsync<WeatherResponse>(url);
            }
            catch
            {
                return null;
            }
        }

        public async Task<WeatherForecast?> GetWeatherForecastAsync(string city, string? countryCode = null)
        {
            try
            {
                var location = BuildLocationQuery(city, countryCode);
                var url = $"https://api.openweathermap.org/data/2.5/forecast?q={location}&appid={_apiKey}&units=metric&lang=ru&cnt=24";
                return await _httpClient.GetFromJsonAsync<WeatherForecast>(url);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> GetWeatherAlertsAsync(string city, string? countryCode = null)
        {
            try
            {
                var location = BuildLocationQuery(city, countryCode);
                var url = $"https://api.openweathermap.org/data/2.5/onecall?q={location}&appid={_apiKey}&exclude=current,minutely,hourly,daily&units=metric&lang=ru";
                var response = await _httpClient.GetFromJsonAsync<dynamic>(url);
                return response?.alerts?.ToString() ?? "Нет активных предупреждений о погоде";
            }
            catch
            {
                return "Не удалось получить данные о предупреждениях";
            }
        }

        private static string BuildLocationQuery(string city, string? countryCode)
      => string.IsNullOrEmpty(countryCode) ? city : $"{city},{countryCode}";
    }
}
