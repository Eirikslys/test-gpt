using Newtonsoft.Json;

namespace PromptApi.Services;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


public class OpenAiApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiApiService(string apiKey)
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }
    
    public async Task<string> SendPromptAsync(string prompt)
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
        
        // if (!response.IsSuccessStatusCode)
        // {
        //     // Handle error here if needed
        //     throw new ApplicationException($"OpenAI API request failed with status code: {response.StatusCode}");
        // }

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}
public class ChatMessage
{
    public string role { get; set; }
    public string content { get; set; }
}