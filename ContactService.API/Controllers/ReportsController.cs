using ContactService.Infrastructure.Kafka;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IKafkaProducerService _kafkaProducer;

    public ReportsController(IKafkaProducerService kafkaProducer)
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
