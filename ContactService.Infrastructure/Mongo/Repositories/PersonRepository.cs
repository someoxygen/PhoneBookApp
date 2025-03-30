using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Mongo.Context;
using MongoDB.Driver;

namespace ContactService.Infrastructure.Mongo.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly IMongoContext _context;

    public PersonRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Person person) =>
        await _context.Persons.InsertOneAsync(person);

    public async Task<List<Person>> GetAllAsync() =>
        await _context.Persons.Find(_ => true).ToListAsync();

    public async Task<Person?> GetByIdAsync(Guid id) =>
        await _context.Persons.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task RemoveAsync(Guid id) =>
        await _context.Persons.DeleteOneAsync(p => p.Id == id);
}
