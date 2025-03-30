using System.Diagnostics.CodeAnalysis;
using ContactService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReportService.Infrastructure.Mongo.Settings;

namespace ContactService.Infrastructure.Mongo.Context;

[ExcludeFromCodeCoverage]
public class MongoContext : IMongoContext
{
    public IMongoCollection<Person> Persons { get; }
    public IMongoCollection<ContactInformation> ContactInformations { get; }

    public MongoContext(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        Persons = database.GetCollection<Person>("Persons");
        ContactInformations = database.GetCollection<ContactInformation>("ContactInformations");
    }
}


