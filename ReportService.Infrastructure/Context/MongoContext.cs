using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Mongo.Settings;

namespace ReportService.Infrastructure.Mongo.Context;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Report> Reports => _database.GetCollection<Report>("Reports");
}
