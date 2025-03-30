using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Mongo.Context;
using MongoDB.Driver;

namespace ContactService.Infrastructure.Mongo.Repositories;

public class ContactInformationRepository : IContactInformationRepository
{
    private readonly IMongoContext _context;

    public ContactInformationRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ContactInformation contactInformation)
    {
        await _context.ContactInformations.InsertOneAsync(contactInformation);
    }

    public async Task<List<ContactInformation>> GetByPersonIdAsync(Guid personId)
    {
        return await _context.ContactInformations
            .Find(c => c.PersonId == personId).ToListAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        await _context.ContactInformations.DeleteOneAsync(c => c.Id == id);
    }
}
