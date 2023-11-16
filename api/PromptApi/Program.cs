using Microsoft.Extensions.Configuration;
using PromptApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // You can adjust this as needed
    .AddEnvironmentVariables() // Optional: Include environment variables in configuration
    .Build();

// Get the value of the API key
string openAiApiKey = builder.Configuration.GetValue<string>("OpenAiApiKey");

// Set the value of the environment variable
Environment.SetEnvironmentVariable("OpenAiApiKey", openAiApiKey);

// Add services to the container.
builder.Services.AddSingleton<IOpenAiApiService, OpenAiApiService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
