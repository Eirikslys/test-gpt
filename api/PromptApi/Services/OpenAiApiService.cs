using Newtonsoft.Json;

namespace PromptApi.Services;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class OpenAiApiService : IOpenAiApiService
{
    private readonly HttpClient _httpClient;
    //private readonly string _apiKey;

    public OpenAiApiService()
    {
        _httpClient = new HttpClient();
        var apiKey = Environment.GetEnvironmentVariable("OpenAiApiKey");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }
    
    public async Task<string> GetPromptAsync(string prompt)
    {
        var messages = new List<ChatMessage>
        {
            new ChatMessage { role = "system", content = "Du er en potensiell kunde til en bilforhandler." },
            new ChatMessage { role = "user", content = prompt }
        };
        
        var requestBody = new
        {
            messages = messages,
            model = "gpt-3.5-turbo" 
        };

        var jsonPayload = JsonConvert.SerializeObject(requestBody);

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        
        if (!response.IsSuccessStatusCode)
        {
            // Handle error here if needed
            throw new ApplicationException($"OpenAI API request failed with status code: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        JObject responseJObject = JObject.Parse(responseContent);

        return responseJObject["choices"][0]["message"]["content"].ToString();;
    }

    public async Task<string> CheckPromptAsync(string initialprompt, string responseprompt)
    {
        string carArrayJson = @"
        [
          {
            ""name"": ""Ford Taurus"",
            ""year"": 2012,
            ""propellant"": ""gasoline"",
            ""seats"": 5,
            ""price"": ""250,000 NOK""
          },
          {
            ""name"": ""Toyota Camry"",
            ""year"": 2015,
            ""propellant"": ""gasoline"",
            ""seats"": 5,
            ""price"": ""300,000 NOK""
          },
          {
            ""name"": ""Honda Civic"",
            ""year"": 2018,
            ""propellant"": ""gasoline"",
            ""seats"": 5,
            ""price"": ""280,000 NOK""
          },
          {
            ""name"": ""Volkswagen Passat"",
            ""year"": 2016,
            ""propellant"": ""diesel"",
            ""seats"": 5,
            ""price"": ""320,000 NOK""
          },
          {
            ""name"": ""Tesla Model S"",
            ""year"": 2019,
            ""propellant"": ""electric"",
            ""seats"": 5,
            ""price"": ""700,000 NOK""
          },
          {
            ""name"": ""Mazda MX-5"",
            ""year"": 2000,
            ""propellant"": ""gasoline"",
            ""seats"": 2,
            ""price"": ""150,000 NOK""
          }
        ]
        ";

        var prompt = $"Kunde: {initialprompt} Ansatt: {responseprompt} Utvalg: {carArrayJson}";
        var messages = new List<ChatMessage>
        {
            new ChatMessage { role = "system", content = "Du er ansvarlig for kvalitetssikring av kundehjelp i en norsk bedrift som driver med salg av biler. Din oppgave er å se på forespørsler som har kommet inn fra kunder, se hvordan våre ansatte svarte på denne meldingen, og vurdere om svaret oppfylte følgende kriterier: 1. kriterie: Svaret skal være høflig og serviceinnstilt, selv om vi ikke kan hjelpe med forespørslene. 2. kriterie: Svaret skal kun anbefale biler som finnes i vårt sortiment. 3. kriterie: Svaret skal korrekt beskrive biler vi har, antall seter i bilen, om bilen går diesel, bensin eller strøm og lignende attributter må være riktige i svaret. Gi svaret i en json med verdiene approved som er true hvis det er godkjent og false hvis det ikke er godkjent, og message." },
            new ChatMessage { role = "user", content = "Kunde: Hei, jeg ser etter en litt sporty familiebil. Jeg liker Opel men er ikke så kresen. Ansatt: Vurder Volkswagen Passat 2016: Romslig, diesel, bra for familier, og prisen er 320,000 NOK. Pålitelig valg for dine behov! utvalg: " + carArrayJson },
            new ChatMessage {role = "assistant", content = "{approved: true, message: Svaret er høflig, anbefaler en bil i vårt sortiment (Volkswagen Passat 2016), og gir korrekt informasjon om bilen. Godkjent.}"},
            new ChatMessage { role = "user", content = prompt }
        };
        
        var requestBody = new
        {
            messages = messages,
            model = "gpt-3.5-turbo" 
        };

        var jsonPayload = JsonConvert.SerializeObject(requestBody);

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        
        if (!response.IsSuccessStatusCode)
        {
            // Handle error here if needed
            throw new ApplicationException($"OpenAI API request failed with status code: {response.StatusCode}");
        }
        
        var responseContent = await response.Content.ReadAsStringAsync();
        JObject responseJObject = JObject.Parse(responseContent);

        return responseJObject["choices"][0]["message"]["content"].ToString();;
    }

}
public class ChatMessage
{
    public string role { get; set; }
    public string content { get; set; }
}