using ContactService.Domain.Entities;

namespace ContactService.Application.Interfaces;

public interface IContactInformationService
{
    Task AddContactInformationAsync(ContactInformation contactInformation);
    Task RemoveContactInformationAsync(Guid id);
    Task<List<ContactInformation>> GetByPersonIdAsync(Guid personId);
}
