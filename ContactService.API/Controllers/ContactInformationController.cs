using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactInformationController : ControllerBase
{
    private readonly IContactInformationService _service;

    public ContactInformationController(IContactInformationService service)
    {
        _service = service;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] ContactInformation contactInformation)
    {
        await _service.AddContactInformationAsync(contactInformation);
        return Ok(contactInformation);
    }

    [HttpDelete("info/{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        await _service.RemoveContactInformationAsync(id);
        return NoContent();
    }

    [HttpGet("person/{personId}")]
    public async Task<ActionResult<List<ContactInformation>>> GetByPersonId(Guid personId)
    {
        var contacts = await _service.GetByPersonIdAsync(personId);
        return Ok(contacts);
    }
}
