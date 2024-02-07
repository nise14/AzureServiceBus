
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(builder =>
{
    // var connectionString = "Endpoint=sb://servicebusdemo2024.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=0hMZAAqusHPZPYGfq4EpEGIq6KJxZQkbI+ASbEoFNRc=";

    builder.AddClient<ServiceBusClient, ServiceBusClientOptions>((_, _, _) =>
    {
        return new ServiceBusClient("servicebusdemo2024.servicebus.windows.net", new DefaultAzureCredential());
    });
    // builder.AddServiceBusClient(connectionString);
});

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
