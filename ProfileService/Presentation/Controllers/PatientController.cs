using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dto.PatientDto;

namespace Presentation.Controllers;

[ApiController]
[Route("api/patient")]
public class PatientController
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }
    
    
    [HttpPost]
    [Route("create-account")]
    public async Task<string> CreateAsync([FromBody] CreatePatientDto  patientDto)
    {
        await _service.CreatePatientAsync(patientDto.FirstName, patientDto.LastName, patientDto.MiddleName, patientDto.DateOfBirth);
        return "patient created";
    }

    [HttpGet]
    [Route("get-patient/{id}")]
    public async Task<Patient> GetPatientByidAsync(Guid id)
    {
        var existingPatient = await _service.GetPatientByidAsync(id);
        
        return existingPatient;
    }

    [HttpGet]
    [Route("get-patient")]
    public async Task<IEnumerable<Patient>> GetPatientsAsync()
    {
        var patients = await _service.GetPatientsAsync();
        
        return patients;
    }

    [HttpPut]
    [Route("update-patient")]
    public async Task UpdatePatientAsync(Guid id, [FromBody] UpdatePatientDto patientDto)
    {
        await _service.UpdatePatientAsync(id, patientDto.FirstName, patientDto.LastName, patientDto.MiddleName, patientDto.DateOfBirth);
    }

    [HttpDelete]
    [Route("delete-patient")]
    public async Task DeletePatientAsync()
    {
        await _service.DeletePatientAsync();
    }
    
}

















