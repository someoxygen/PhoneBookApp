using ContactService.Application.Services;
using ContactService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactInformationController : ControllerBase
{
    private readonly ContactInformationService _service;

    public ContactInformationController(ContactInformationService service)
    {
        _service = service;
    }

    // İletişim bilgisi ekle
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] ContactInformation contactInformation)
    {
        await _service.AddContactInformationAsync(contactInformation);
        return Ok(contactInformation);
    }

    // İletişim bilgisi kaldır
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        await _service.RemoveContactInformationAsync(id);
        return NoContent();
    }

    // Bir kişiye ait iletişim bilgilerini getir
    [HttpGet("person/{personId}")]
    public async Task<ActionResult<List<ContactInformation>>> GetByPersonId(Guid personId)
    {
        var contacts = await _service.GetByPersonIdAsync(personId);
        return Ok(contacts);
    }
}
