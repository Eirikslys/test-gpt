namespace PromptApi.Services;

public interface IOpenAiApiService
{
    Task<string> GetPromptAsync(string prompt);
    Task<string> CheckPromptAsync(string initialPrompt, string responsePrompt);
}