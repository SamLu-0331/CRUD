using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableUpdateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string BirthDate { get; set; } = null!;
        public List<Guid> DoctorIds { get; set; }

    }
}