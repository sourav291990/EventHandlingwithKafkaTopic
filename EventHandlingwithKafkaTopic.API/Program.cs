using EventHandlingwithKafkaTopic.API;
using EventHandlingwithKafkaTopic.Services.Contracts;
using EventHandlingwithKafkaTopic.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IKafkaProducerService>(serviceProvider =>
{
    var bootstrapServers = "localhost:9092";
    var logger = serviceProvider.GetRequiredService<ILogger<KafkaPublisherService>>();
    return new KafkaPublisherService(bootstrapServers, logger);
});

builder.Services.AddSingleton<IKafkaConsumerService>(serviceProvider =>
{
    var bootstrapServers = "localhost:9092";
    var logger = serviceProvider.GetRequiredService<ILogger<KafkaConsumerService>>();
    return new KafkaConsumerService(logger, bootstrapServers);
});

builder.Services.AddHostedService<KafkaConsumerBackgroundService>();


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
