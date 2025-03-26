using Microsoft.AspNetCore.Mvc;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Mongo.Repositories;

namespace ReportService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly ReportRepository _reportRepository;

    public ReportController(ReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    // Sistemin oluşturduğu raporların listelenmesi
    [HttpGet("all")]
    public async Task<ActionResult<List<Report>>> GetAllReports()
    {
        var reports = await _reportRepository.GetAllAsync();
        return Ok(reports);
    }

    // Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi
    [HttpGet("{id}")]
    public async Task<ActionResult<Report>> GetReportById(Guid id)
    {
        var report = await _reportRepository.GetByIdAsync(id);
        if (report is null)
            return NotFound();

        return Ok(report);
    }
}
