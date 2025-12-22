using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class Doctor : ProfileModel
{
    public DateOnly StartWorkingDate { get; set; }
    public DoctorStatus Status {get; set;}
    public  Guid SpecializationId { get; set; }
    public  Guid OfficeId { get; set; }
}