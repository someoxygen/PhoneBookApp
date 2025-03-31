using Microsoft.Extensions.Logging;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using Shared.Events;
using ContactService.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace ReportService.Infrastructure.Kafka.Handlers;

public class ReportRequestedHandler : IReportRequestedHandler
{
    private readonly IReportRepository _reportRepository;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReportRequestedHandler> _logger;

    public ReportRequestedHandler(
        IReportRepository reportRepository,
        HttpClient httpClient,
        ILogger<ReportRequestedHandler> logger)
    {
        _reportRepository = reportRepository;
        _httpClient = httpClient;
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

        await _reportRepository.CreateAsync(report);

        var persons = await _httpClient.GetFromJsonAsync<List<Person>>("http://localhost:5211/api/person/all");

        if (persons == null || persons.Count == 0)
        {
            _logger.LogWarning("Hiç kişi bulunamadı.");
            return;
        }

        var reportDetails = new List<ReportDetail>();

        foreach (var person in persons)
        {
            var contacts = await _httpClient.GetFromJsonAsync<List<ContactInformation>>(
                $"http://localhost:5211/api/contactinformation/person/{person.Id}");

            if (contacts == null) continue;

            var personLocations = contacts
                .Where(c => c.Type == ContactType.Location && !string.IsNullOrWhiteSpace(c.Content))
                .Select(c => c.Content)
                .Distinct();

            var phoneCount = contacts.Count(c => c.Type == ContactType.PhoneNumber);

            foreach (var location in personLocations)
            {
                var existing = reportDetails.FirstOrDefault(r => r.Location == location);
                if (existing != null)
                {
                    existing.PersonCount++;
                    existing.PhoneNumberCount += phoneCount;
                }
                else
                {
                    reportDetails.Add(new ReportDetail
                    {
                        Location = location,
                        PersonCount = 1,
                        PhoneNumberCount = phoneCount
                    });
                }
            }
        }

        report.Status = ReportStatus.Completed;
        report.Details = reportDetails;

        await _reportRepository.UpdateAsync(report);

        _logger.LogInformation($"Rapor tamamlandı: {report.Id}");
    }
}
