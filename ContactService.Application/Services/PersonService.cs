using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;

namespace ContactService.Application.Services;

public class PersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task CreatePersonAsync(Person person)
    {
        // İş mantığı kontrolü buraya eklenebilir (örneğin validasyon vb.)
        await _personRepository.CreateAsync(person);
    }

    public async Task<List<Person>> GetAllPersonsAsync()
    {
        return await _personRepository.GetAllAsync();
    }

    public async Task<Person?> GetPersonByIdAsync(Guid id)
    {
        return await _personRepository.GetByIdAsync(id);
    }

    public async Task RemovePersonAsync(Guid id)
    {
        await _personRepository.RemoveAsync(id);
    }
}
