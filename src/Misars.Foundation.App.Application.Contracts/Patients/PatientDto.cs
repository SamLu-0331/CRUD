using System;
using Volo.Abp.Application.Dtos;

namespace Misars.Foundation.App.Patients;

public class PatientDto : AuditedEntityDto<Guid>
{
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public PatientType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }
}
