using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // You can adjust this as needed
    .AddEnvironmentVariables() // Optional: Include environment variables in configuration
    .Build();

// Define your configuration settings using a POCO class
var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

builder.Services.AddSingleton(appSettings);

// Add services to the container.

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

// Define a POCO class for your configuration settings
public class AppSettings
{
    public string OpenAiApiKey { get; set; }
}