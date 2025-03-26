using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Events;
using System.Text.Json;

namespace ReportService.Infrastructure.Kafka.Consumers;

public class ReportRequestedConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly ILogger<ReportRequestedConsumer> _logger;

    public ReportRequestedConsumer(IConfiguration configuration, ILogger<ReportRequestedConsumer> logger)
    {
        _logger = logger;
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = "report-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        return Task.CompletedTask;
    }

    private void StartConsumerLoop(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe("report-requested");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);

                var reportRequestedEvent = JsonSerializer.Deserialize<ReportRequestedEvent>(consumeResult.Message.Value);

                _logger.LogInformation($"Yeni rapor talebi alındı. ReportId: {reportRequestedEvent.ReportId}");

                // Burada raporu hazırlama mantığını yazacağız.
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hata: {ex.Message}");
            }
        }

        consumer.Close();
    }
}
