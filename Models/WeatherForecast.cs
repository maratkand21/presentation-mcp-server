namespace WeatherMcpServer.Models
{
    public record WeatherForecast(
           string Cod,
           int Message,
           int Cnt,
           List<ForecastItem> List,
           City City
       );

    public record ForecastItem(
        long Dt,
        Main Main,
        List<Weather> Weather,
        Clouds Clouds,
        Wind Wind,
        int Visibility,
        double Pop,
        Sys Sys,
        string DtTxt
    );

    public record City(
        long Id,
        string Name,
        Coord Coord,
        string Country,
        int Population,
        int Timezone,
        long Sunrise,
        long Sunset
    );
}
