using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task CreatePersonAsync(Person person)
        => await _personRepository.CreateAsync(person);

    public async Task<List<Person>> GetAllPersonsAsync()
        => await _personRepository.GetAllAsync();

    public async Task<Person?> GetPersonByIdAsync(Guid id)
        => await _personRepository.GetByIdAsync(id);

    public async Task RemovePersonAsync(Guid id)
        => await _personRepository.RemoveAsync(id);
}
