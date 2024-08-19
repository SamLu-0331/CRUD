using System;
using System.ComponentModel.DataAnnotations;

namespace Misars.Foundation.App.Patients;

public class CreateUpdatePatientDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public PatientType Type { get; set; } = PatientType.Undefined;

    [Required]
    [DataType(DataType.Date)]
    public DateTime PublishDate { get; set; } = DateTime.Now;

    [Required]
    public float Price { get; set; }
    public Guid DoctorId { get; set; }

}
