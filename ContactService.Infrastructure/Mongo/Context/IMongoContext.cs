using ContactService.Domain.Entities;
using MongoDB.Driver;

namespace ContactService.Infrastructure.Mongo.Context;
public interface IMongoContext
{
    IMongoCollection<Person> Persons { get; }
    IMongoCollection<ContactInformation> ContactInformations { get; }
}
