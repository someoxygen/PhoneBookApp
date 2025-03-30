using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Shared.Events;

namespace ContactService.Infrastructure.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly ProducerConfig _config;

    public KafkaProducerService(IConfiguration configuration)
    {
        _config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
    }

    public async Task ProduceReportRequestedEventAsync(ReportRequestedEvent @event)
    {
        using var producer = new ProducerBuilder<Null, string>(_config).Build();
        var topic = "report-requested";
        var message = JsonSerializer.Serialize(@event);

        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
    }
}
