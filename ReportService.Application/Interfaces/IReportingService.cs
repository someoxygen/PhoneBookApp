using ReportService.Domain.Entities;

namespace ReportService.Application.Interfaces;

public interface IReportingService
{
    Task<List<Report>> GetAllReportsAsync();
    Task<Report?> GetReportByIdAsync(Guid id);
    Task CreateReportAsync(Report report);
    Task UpdateReportAsync(Report report);
}
