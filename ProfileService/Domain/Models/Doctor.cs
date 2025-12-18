using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class Doctor : ProfileModel
{
    //public string/Guid SpecializationId { get; set; }
    //public string/Guid OfficeId { get; set; }
    public DateOnly StartWorkingDate { get; set; }
    public DoctorStatus Status {get; set;}
}