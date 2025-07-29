using WeatherMcpServer.Models;

namespace WeatherMcpServer.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetCurrentWeatherAsync(string city, string? countryCode = null);
        Task<WeatherForecast?> GetWeatherForecastAsync(string city, string? countryCode = null);
        Task<string?> GetWeatherAlertsAsync(string city, string? countryCode = null);
        void SetApiKey(string? apiKey);
    }
}
