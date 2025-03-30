using MongoDB.Driver;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Context;

namespace ReportService.Infrastructure.Mongo.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly IMongoContext _context;

    public ReportRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Report report) =>
        await _context.Reports.InsertOneAsync(report);

    public async Task UpdateAsync(Report report) =>
        await _context.Reports.ReplaceOneAsync(r => r.Id == report.Id, report);

    public async Task<Report?> GetByIdAsync(Guid id) =>
        await _context.Reports.Find(r => r.Id == id).FirstOrDefaultAsync();

    public async Task<List<Report>> GetAllAsync() =>
        await _context.Reports.Find(_ => true).ToListAsync();
}
