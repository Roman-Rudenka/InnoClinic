using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentattion.DTOs;

namespace Presentation.Controllers;

[ApiController]
[Route("api/offices")]
public class OfficeController(IOfficeService officeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddOffice([FromBody] CreateOfficeDto dto, CancellationToken cancellationToken = default)
    {
        var office = await officeService.AddOfficeAsync(dto.Address, dto.PhoneNumber, cancellationToken);
        
        return CreatedAtAction(nameof(GetOfficeById), new { id = office.Id }, office);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOfficeById(Guid id, CancellationToken cancellationToken = default)
    {
        var office = await officeService.GetOfficeByIdAsync(id, cancellationToken);

        return Ok(office);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOffices(CancellationToken cancellationToken = default)
    {
        var offices = await officeService.GetOffices(cancellationToken);
        
        return Ok(offices);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOffice(Guid id, [FromBody] UpdateOfficeDto dto, CancellationToken cancellationToken = default)
    {
        var updated = await officeService.UpdateOfficeAsync(id, dto, cancellationToken);
        
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveOffice(Guid id, CancellationToken cancellationToken = default)
    {
        await officeService.RemoveOfficeAsync(id, cancellationToken);
        
        return NoContent();
    }

    [HttpPut("{id}/activate")]
    public async Task<IActionResult> ActivateOffice(Guid id, CancellationToken cancellationToken = default)
    {
        var office = await officeService.GetOfficeByIdAsync(id, cancellationToken);

        officeService.ChangeOfficeStatusToIsActive(office);
        await officeService.UpdateOfficeAsync(id, new UpdateOfficeDto { Status = true }, cancellationToken);

        return Ok(office);
    }

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> DeactivateOffice(Guid id, CancellationToken cancellationToken = default)
    {
        var office = await officeService.GetOfficeByIdAsync(id, cancellationToken);

        officeService.ChangeOfficeStatusToIsNotActive(office);
        await officeService.UpdateOfficeAsync(id, new UpdateOfficeDto { Status = false }, cancellationToken);

        return Ok(office);
    }
    
    [HttpGet("by-address")]
    public async Task<IActionResult> GetOfficeByAddress([FromQuery] string address, CancellationToken cancellationToken)
    {
        var office = await officeService.GetOfficeByAddressAsync(address, cancellationToken);
        return Ok(office);
    }
}
