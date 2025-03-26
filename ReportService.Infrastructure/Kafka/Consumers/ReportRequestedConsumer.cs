using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Mongo.Repositories;
using Shared.Events;

namespace ReportService.Infrastructure.Kafka.Consumers;
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

                _logger.LogInformation($"Rapor işleniyor: {reportRequestedEvent.ReportId}");

                using var scope = _serviceScopeFactory.CreateScope();
                var reportRepository = scope.ServiceProvider.GetRequiredService<ReportRepository>();

                var report = new Report
                {
                    Id = reportRequestedEvent.ReportId,
                    RequestedAt = reportRequestedEvent.RequestedAt,
                    Status = ReportStatus.Preparing
                };

                await reportRepository.CreateAsync(report);

                // (Buraya rapor oluşturma iş mantığını ekleyeceğiz, şimdilik simüle edelim)
                await Task.Delay(5000); // rapor oluşturma simülasyonu

                report.Status = ReportStatus.Completed;
                report.Details.Add(new ReportDetail
                {
                    Location = "İstanbul",
                    PersonCount = 10,
                    PhoneNumberCount = 15
                });

                await reportRepository.UpdateAsync(report);

                _logger.LogInformation($"Rapor tamamlandı: {report.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor işlenirken hata oluştu!");
            }
        }

        consumer.Close();
    }
}
