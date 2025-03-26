using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;

namespace ContactService.Application.Services;

public class ContactInformationService
{
    private readonly IContactInformationRepository _repository;

    public ContactInformationService(IContactInformationRepository repository)
    {
        _repository = repository;
    }

    public async Task AddContactInformationAsync(ContactInformation contactInformation)
    {
        // İş mantığı (validasyon vs.) eklenebilir
        await _repository.AddAsync(contactInformation);
    }

    public async Task RemoveContactInformationAsync(Guid id)
    {
        await _repository.RemoveAsync(id);
    }

    public async Task<List<ContactInformation>> GetByPersonIdAsync(Guid personId)
    {
        return await _repository.GetByPersonIdAsync(personId);
    }
}
