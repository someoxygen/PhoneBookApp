using Microsoft.Extensions.Logging;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using Shared.Events;

namespace ReportService.Infrastructure.Kafka.Handlers;

public class ReportRequestedHandler : IReportRequestedHandler
{
    private readonly IReportRepository _repository;
    private readonly ILogger<ReportRequestedHandler> _logger;

    public ReportRequestedHandler(IReportRepository repository, ILogger<ReportRequestedHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(ReportRequestedEvent evt)
    {
        var report = new Report
        {
            Id = evt.ReportId,
            RequestedAt = evt.RequestedAt,
            Status = ReportStatus.Preparing
        };

        await _repository.CreateAsync(report);

        // Simülasyon (gerçek rapor oluşturma zamanı)
        await Task.Delay(100); // testte zaman kaybetmemek için küçük tutabilirsin

        report.Status = ReportStatus.Completed;
        report.Details.Add(new ReportDetail
        {
            Location = "İstanbul",
            PersonCount = 10,
            PhoneNumberCount = 15
        });

        await _repository.UpdateAsync(report);

        _logger.LogInformation($"Rapor tamamlandı: {report.Id}");
    }
}
