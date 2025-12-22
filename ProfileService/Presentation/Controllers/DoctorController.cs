using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorController(IDoctorService doctorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await doctorService.GetDoctorsAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await doctorService.GetDoctorByIdAsync(id, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Doctor not found.");
        }
    }

    [HttpGet("by-specialization")]
    public async Task<IActionResult> GetBySpecialization([FromQuery] string name, CancellationToken ct)
    {
        var result = await doctorService.GetDoctorsBySpecialisationAsync(name, ct);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Doctor doctor, CancellationToken ct)
    {
        try
        {
            await doctorService.UpdateDoctorAsync(doctor, ct);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await doctorService.DeleteDoctorAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}