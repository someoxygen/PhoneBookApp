using MongoDB.Driver;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Mongo.Settings;
using Microsoft.Extensions.Options;

namespace ContactService.Infrastructure.Mongo.Context;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Person> Persons => _database.GetCollection<Person>("Persons");
    public IMongoCollection<ContactInformation> ContactInformations => _database.GetCollection<ContactInformation>("ContactInformations");
}
