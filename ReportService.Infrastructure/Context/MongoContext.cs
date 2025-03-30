using System.Diagnostics.CodeAnalysis;
using ReportService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReportService.Infrastructure.Mongo.Settings;

namespace ReportService.Infrastructure.Context;
[ExcludeFromCodeCoverage]
public class MongoContext : IMongoContext
{
    public IMongoCollection<Report> Reports { get; }

    public MongoContext(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        Reports = database.GetCollection<Report>("Reports");
    }
}


