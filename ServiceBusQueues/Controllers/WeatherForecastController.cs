using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace ServiceBusQueues.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ServiceBusClient _client;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ServiceBusClient client)
    {
        _logger = logger;
        _client = client;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public async Task Post(WeatherForecast data)
    {
        var sender = _client.CreateSender("add-weather-data");
        var body = JsonSerializer.Serialize(data);
        var message = new ServiceBusMessage(body);
        if (body.Contains("scheduled"))
        {
            message.ScheduledEnqueueTime = DateTimeOffset.UtcNow.AddSeconds(15);
        }

        if (body.Contains("tts"))
        {
            message.TimeToLive = TimeSpan.FromSeconds(20);
        }

        await sender.SendMessageAsync(message);
    }
}
