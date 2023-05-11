using BackEnd.Models;
using Newtonsoft.Json.Linq;
namespace BackEnd.Services
{
    public interface IWeatherService
	{
        public Task<List<DailyForecast>> GetWeatherForecastAsync(double requestLatitude, double requestLongitude);
	}

	public class WeatherService : IWeatherService
	{
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DailyForecast>> GetWeatherForecastAsync(double requestLatitude, double requestLongitude)
        {
            string endpoint = @$"https://api.open-meteo.com/v1/forecast?latitude={requestLatitude}&longitude={requestLongitude}&daily=temperature_2m_max,temperature_2m_min,sunrise,sunset,uv_index_max,uv_index_clear_sky_max,windspeed_10m_max&timezone=Europe%2FBerlin";
            string newEndpoint = $@"https://api.open-meteo.com/v1/forecast?latitude={requestLatitude}&longitude={requestLongitude}&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,uv_index_max,windspeed_10m_max&forecast_days=3&timezone=Europe%2FBerlin";

            HttpResponseMessage response = await _httpClient.GetAsync(newEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve weather data. Status code: {response.StatusCode}");
            }

            string content = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(content);

            List<DailyForecast> dailyForecasts = new List<DailyForecast>();

            JArray? time = (JArray)jsonObject["daily"]["time"];
            JArray? weathercode = (JArray)jsonObject["daily"]["weathercode"];
            JArray? temperature2mMax = (JArray)jsonObject["daily"]["temperature_2m_max"];
            JArray? temperature2mMin = (JArray)jsonObject["daily"]["temperature_2m_min"];
            JArray? sunrise = (JArray)jsonObject["daily"]["sunrise"];
            JArray? sunset = (JArray)jsonObject["daily"]["sunset"];
            JArray? uvIndexMax = (JArray)jsonObject["daily"]["uv_index_max"];
            JArray? uvIndexClearSkyMax = (JArray)jsonObject["daily"]["uv_index_clear_sky_max"];
            JArray? windspeed10mMax = (JArray)jsonObject["daily"]["windspeed_10m_max"];

            for (int i = 0; i < time.Count; i++)
            {
                DailyForecast dailyForecast = new DailyForecast
                {
                    Date = DateTime.Parse((string)time[i]),
                    Summary = _getWeatherSummaryFromWeatherCode((int)weathercode[i]),
                    MaxTemperature = (double)temperature2mMax[i],
                    MinTemperature = (double)temperature2mMin[i],
                    Sunrise = DateTime.Parse((string)sunrise[i]),
                    Sunset = DateTime.Parse((string)sunset[i]),
                    MaxUVIndex = (double)uvIndexMax[i],
                    //MaxUVIndexClearSky = (double)uvIndexClearSkyMax[i],
                    MaxWindSpeed = (double)windspeed10mMax[i]
                };
                dailyForecasts.Add(dailyForecast);
            }

            return dailyForecasts;
        }

        private string _getWeatherSummaryFromWeatherCode(int weathercode)
        {
            switch (weathercode)
            {
                case 0:
                    return "Clear sky";
                case 1:
                    return "Mainly clear";
                case 2:
                    return "Partly cloudy";
                case 3:
                    return "Overcast";
                case 45:
                    return "Fog";
                case 48:
                    return "Depositing rime Fog";
                case 51:
                    return "Light drizzle";
                case 53:
                    return "Moderate drizzle";
                case 55:
                    return "Dense drizzle";
                case 56:
                    return "Light freezing drizzle";
                case 57:
                    return "Dense freezing drizzle";
                case 61:
                    return "Slight rain";
                case 63:
                    return "Moderate rain";
                case 65:
                    return "Heavy rain";
                case 66:
                    return "Light freezing rain";
                case 67:
                    return "Heavy freezing rain";
                case 71:
                    return "Slight snow fall";
                case 73:
                    return "Moderate snow fall";
                case 75:
                    return "Heavy snow fall";
                case 77:
                    return "Snow grains";
                case 80:
                    return "Slight rain showers";
                case 81:
                    return "Moderate rain showers";
                case 82:
                    return "Violent rain showers";
                case 85:
                    return "Slight snow showers";
                case 86:
                    return "Heavy snow showers";
                case 95:
                    return "Thunderstorm";
                case 96:
                    return "Thunderstorm with light hail";
                case 99:
                    return "Thunderstorm with heavy hail";
                default:
                    return "Invalid weathercode";
            }
        }
    }
}

