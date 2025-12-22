using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController(IServicesService servicesService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var list = await servicesService.GetAllActiveAsync(ct);
        return Ok(list);
    }

    [HttpGet("by-specialization/{specializationId:guid}")]
    public async Task<IActionResult> GetBySpecialization(Guid specializationId, CancellationToken ct)
    {
        var list = await servicesService.GetBySpecializationAsync(specializationId, ct);
        return Ok(list);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceDto dto, CancellationToken ct)
    {
        await servicesService.CreateServiceAsync(dto, ct);
        return Ok(new { message = "Service created" });
    }
}