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
    
    
    // [HttpPost]
    // [Route("create-account")]
    // public async Task<string> CreateAsync([FromBody] CreatePatientDto patientDto, CancellationToken cancellationToken)
    // {
    //     await _service.CreatePatientAsync(patientDto.FirstName, patientDto.LastName, patientDto.MiddleName, patientDto.DateOfBirth, cancellationToken);
    //     return "patient created";
    // }

    [HttpGet]
    [Route("get-patient/{id}")]
    public async Task<Patient> GetPatientByidAsync(Guid id, CancellationToken cancellationToken)
    {
        var existingPatient = await _service.GetPatientByidAsync(id, cancellationToken);
        
        return existingPatient;
    }

    [HttpGet]
    [Route("get-patients")]
    public async Task<IEnumerable<Patient>> GetPatientsAsync(CancellationToken cancellationToken)
    {
        var patients = await _service.GetPatientsAsync(cancellationToken);
        
        return patients;
    }

    [HttpPut]
    [Route("update-patient")]
    public async Task UpdatePatientAsync(Guid id, [FromBody] UpdatePatientDto patientDto,CancellationToken cancellationToken)
    {
        await _service.UpdatePatientAsync(id, patientDto.FirstName, patientDto.LastName, patientDto.MiddleName,
            patientDto.DateOfBirth, cancellationToken);
    }

    // [HttpDelete]
    // [Route("delete-patient")]
    // public async Task DeletePatientAsync(Guid  id, CancellationToken cancellationToken)
    // {
    //     await _service.DeletePatientAsync(id, cancellationToken);
    // }
}

















