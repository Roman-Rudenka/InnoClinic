using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
{
    [HttpGet("slots")]
    public async Task<IActionResult> GetSlots([FromQuery] Guid doctorId, [FromQuery] DateOnly date, CancellationToken ct)
    {
        var slots = await appointmentService.GetFreeSlotsAsync(doctorId, date, ct);
        return Ok(slots);
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto, CancellationToken ct)
    {
        await appointmentService.CreateAppointmentAsync(dto, ct);
        return Ok(new { message = "Appointment booked successfully" });
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyAppointments(CancellationToken ct)
    {
        var appointments = await appointmentService.GetMyAppointmentsAsync(ct);
        return Ok(appointments);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] bool isApproved, CancellationToken ct)
    {
        await appointmentService.ChangeStatusAsync(id, isApproved, ct);
        return Ok(new { message = isApproved ? "Appointment approved" : "Appointment rejected" });
    }
}