using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Misars.Foundation.App.Patients;

public class Patient : AuditedAggregateRoot<Guid>
{
    public Guid DoctorId { get; set; }

    public string Name { get; set; } = string.Empty;

    public PatientType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }
}
