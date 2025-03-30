using ReportService.Domain.Entities;
using MongoDB.Driver;

namespace ReportService.Infrastructure.Context;
public interface IMongoContext
{
    IMongoCollection<Report> Reports { get; }
}
