using Microsoft.AspNetCore.Mvc;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;

namespace ReportService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportingController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportingController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Report>>> GetAllReports()
        => Ok(await _reportingService.GetAllReportsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Report>> GetReportById(Guid id)
    {
        var report = await _reportingService.GetReportByIdAsync(id);
        if (report is null)
            return NotFound();

        return Ok(report);
    }
}
