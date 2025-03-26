using ContactService.Domain.Entities;

namespace ContactService.Application.Interfaces;

public interface IContactInformationRepository
{
    Task AddAsync(ContactInformation contactInformation);
    Task RemoveAsync(Guid id);
    Task<List<ContactInformation>> GetByPersonIdAsync(Guid personId);
}
