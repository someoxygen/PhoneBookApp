using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;

public class ContactInformationService : IContactInformationService
{
    private readonly IContactInformationRepository _repository;

    public ContactInformationService(IContactInformationRepository repository)
    {
        _repository = repository;
    }

    public async Task AddContactInformationAsync(ContactInformation contactInformation)
        => await _repository.AddAsync(contactInformation);

    public async Task RemoveContactInformationAsync(Guid id)
        => await _repository.RemoveAsync(id);

    public async Task<List<ContactInformation>> GetByPersonIdAsync(Guid personId)
        => await _repository.GetByPersonIdAsync(personId);
}
