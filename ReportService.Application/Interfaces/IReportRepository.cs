using ReportService.Domain.Entities;

namespace ReportService.Application.Interfaces;

public interface IReportRepository
{
    Task<List<Report>> GetAllAsync();
    Task<Report?> GetByIdAsync(Guid id);
    Task CreateAsync(Report report);
    Task UpdateAsync(Report report);
}
