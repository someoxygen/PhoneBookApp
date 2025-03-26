using ContactService.Domain.Entities;

namespace ContactService.Application.Interfaces;

public interface IPersonRepository
{
    Task CreateAsync(Person person);
    Task<List<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(Guid id);
    Task RemoveAsync(Guid id);
}
