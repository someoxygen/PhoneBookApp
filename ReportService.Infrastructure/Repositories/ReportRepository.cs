using MongoDB.Driver;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Mongo.Context;

namespace ReportService.Infrastructure.Mongo.Repositories;

public class ReportRepository
{
    private readonly MongoContext _context;

    public ReportRepository(MongoContext context)
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
