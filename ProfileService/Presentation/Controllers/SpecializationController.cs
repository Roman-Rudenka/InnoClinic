using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/specializations")]
public class SpecializationController(ISpecializationService specializationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Specialization specialization, CancellationToken ct)
    {
        try
        {
            await specializationService.CreateSpecializationAsync(specialization, ct);
            return CreatedAtAction(nameof(GetById), new { id = specialization.Id }, specialization);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await specializationService.GetSpecializationsAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await specializationService.GetSpecializationByIdAsync(id, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> GetByName([FromQuery] string name, CancellationToken ct)
    {
        try
        {
            var result = await specializationService.GetSpecializationByNameAsync(name, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Specialization specialization, CancellationToken ct)
    {
        await specializationService.UpdateSpecializationAsync(specialization, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await specializationService.DeleteSpecializationAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}