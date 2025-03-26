using ContactService.Infrastructure.Kafka;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

namespace ContactService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly KafkaProducerService _kafkaProducer;

    public ReportsController(KafkaProducerService kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    [HttpPost("request-report")]
    public async Task<IActionResult> RequestReport()
    {
        var reportEvent = new ReportRequestedEvent();

        await _kafkaProducer.ProduceReportRequestedEventAsync(reportEvent);

        return Ok(new { Message = "Rapor talebi alındı, işleniyor.", ReportId = reportEvent.ReportId });
    }
}
