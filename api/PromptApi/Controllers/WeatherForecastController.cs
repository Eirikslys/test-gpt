using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PromptApi.Services;

namespace PromptApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private OpenAiApiService _service;
    
    public WeatherForecastController(IConfiguration configuration)
    {
        _configuration = configuration;
        string apiKey = _configuration["OpenAiApiKey"];
        _service = new OpenAiApiService(apiKey);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        string response = await _service.SendPromptAsync("Skriv en melding på maks 200 tegn hvor du beskriver hva slags bil du ser etter og spør om de har noe som passer beskrivelsen. Bruk et muntlig språk");

        if (string.IsNullOrWhiteSpace(response))
        {
            return BadRequest("No string returned");
        }

        return Ok(response);
    }
    
    // [HttpGet]
    // public IActionResult Get()
    // {
    //     
    //     string apiKey = _configuration["OpenAiApiKey"];
    //
    //     if (string.IsNullOrWhiteSpace(apiKey))
    //     {
    //         return BadRequest("API key is missing or empty.");
    //     }
    //
    //     return Ok(apiKey);
    // }



    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<WeatherForecast> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateTime.Now.AddDays(index),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }
}

