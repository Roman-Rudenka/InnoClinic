using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/results")]
public class ResultsController(IResultService resultService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateResultDto dto, CancellationToken ct)
    {
        var result = await resultService.CreateAsync(dto, ct);
        return Ok(result);
    }
    
    [HttpGet("by-appointment/{appointmentId:guid}")]
    public async Task<IActionResult> GetByAppointmentId(Guid appointmentId, CancellationToken ct)
    {
        var result = await resultService.GetByAppointmentIdAsync(appointmentId, ct);
        if (result == null) return NotFound("Result not found for this appointment.");
        
        return Ok(result);
    }
}