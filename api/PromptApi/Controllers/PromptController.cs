using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PromptApi.Services;

namespace PromptApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PromptController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IOpenAiApiService _service;
    
    public PromptController(IConfiguration configuration, IOpenAiApiService service)
    {
        _configuration = configuration;
        string apiKey = _configuration["OpenAiApiKey"];
        _service = service;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        string response = await _service.GetPromptAsync("Skriv en melding på maks 200 tegn hvor du beskriver hva slags bil du ser etter og spør om de har noe som passer beskrivelsen. Bruk et muntlig språk");

        if (string.IsNullOrWhiteSpace(response))
        {
            return BadRequest("No string returned");
        }

        return Ok(response);
    }
    
    [HttpGet("check-prompt")]
    public async Task<IActionResult> CheckPrompt(string initalPrompt, string responsePrompt)
    {
        string response = await _service.CheckPromptAsync(initalPrompt, responsePrompt);
        return Ok(response);
    }
}

