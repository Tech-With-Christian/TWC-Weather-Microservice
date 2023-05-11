using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeteoWeatherController : Controller
    {
        private readonly ILogger<MeteoWeatherController> _logger;
        private readonly IWeatherService _weatherService;

        public MeteoWeatherController(ILogger<MeteoWeatherController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        
        public async Task<IActionResult> Get([FromQuery] double latitude, [FromQuery] double longitude)
        {
            List<DailyForecast> dailyForecasts = await _weatherService.GetWeatherForecastAsync(latitude, longitude);

            return Ok(dailyForecasts);
        }
    }
}

