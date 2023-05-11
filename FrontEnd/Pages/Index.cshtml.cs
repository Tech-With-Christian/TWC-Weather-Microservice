using Dapr.Client;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DaprClient _daprClient;
    private const string DefaultLatitude = "55.33";
    private const string DefaultLongitude = "10.37";

    [BindProperty]
    public string Latitude { get; set; } = DefaultLatitude;

    [BindProperty]
    public string Longitude { get; set; } = DefaultLongitude;

    public IndexModel(ILogger<IndexModel> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    //public async Task OnGet()
    //{
    //    var forecasts = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
    //        HttpMethod.Get,
    //        "BackEnd",
    //        "weatherforecast");
    //    ViewData["WeatherForecastData"] = forecasts;
    //    ViewData["WeatherForecastCount"] = forecasts.Count();
    //}

    public async Task OnGet(string latitude = DefaultLatitude, string longitude = DefaultLongitude)
    {
        var forecasts = await _daprClient.InvokeMethodAsync<IEnumerable<DailyForecast>>(
            HttpMethod.Get,
            "BackEnd",
            $"meteoweather?latitude={latitude}&longitude={longitude}");
        ViewData["WeatherForecastData"] = forecasts;
        ViewData["WeatherForecastCount"] = forecasts.Count();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        return RedirectToPage("./Index", new { latitude = Latitude, longitude = Longitude });
    }
}

