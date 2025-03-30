using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;

namespace ReportService.Application.Services;

public class ReportingService : IReportingService
{
    private readonly IReportRepository _repository;

    public ReportingService(IReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Report>> GetAllReportsAsync()
        => await _repository.GetAllAsync();

    public async Task<Report?> GetReportByIdAsync(Guid id)
        => await _repository.GetByIdAsync(id);

    public async Task CreateReportAsync(Report report)
        => await _repository.CreateAsync(report);

    public async Task UpdateReportAsync(Report report)
        => await _repository.UpdateAsync(report);
}
