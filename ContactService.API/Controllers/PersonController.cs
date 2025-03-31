using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePerson([FromBody] Person person)
    {
        await _personService.CreatePersonAsync(person);
        return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Person>>> GetAllPersons()
        => Ok(await _personService.GetAllPersonsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPersonById(Guid id)
    {
        var person = await _personService.GetPersonByIdAsync(id);
        if (person is null)
            return NotFound();

        return Ok(person);
    }

    [HttpDelete("person/{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await _personService.RemovePersonAsync(id);
        return NoContent();
    }
}
