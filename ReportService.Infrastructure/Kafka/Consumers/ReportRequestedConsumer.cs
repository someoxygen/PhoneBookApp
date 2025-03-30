using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportService.Application.Interfaces;
using Shared.Events;
using System.Text.Json;

public class ReportRequestedConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly ILogger<ReportRequestedConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ReportRequestedConsumer(IConfiguration configuration,
                                   ILogger<ReportRequestedConsumer> logger,
                                   IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
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

    private async Task StartConsumerLoop(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe("report-requested");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);
                var reportRequestedEvent = JsonSerializer.Deserialize<ReportRequestedEvent>(consumeResult.Message.Value);

                using var scope = _serviceScopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IReportRequestedHandler>();

                await handler.HandleAsync(reportRequestedEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor işlenirken hata oluştu!");
            }
        }

        consumer.Close();
    }
}
