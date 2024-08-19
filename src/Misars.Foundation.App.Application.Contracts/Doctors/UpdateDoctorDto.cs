using System;
using System.ComponentModel.DataAnnotations;

namespace Misars.Foundation.App.Doctors;
public class UpdateDoctorDto
{
    [Required]
    [StringLength(DoctorConsts.MaxNameLength)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }
    
    public string? ShortBio { get; set; }
}
