using ContactService.Domain.Entities;

namespace ContactService.Application.Interfaces;

public interface IPersonService
{
    Task CreatePersonAsync(Person person);
    Task<List<Person>> GetAllPersonsAsync();
    Task<Person?> GetPersonByIdAsync(Guid id);
    Task RemovePersonAsync(Guid id);
}
