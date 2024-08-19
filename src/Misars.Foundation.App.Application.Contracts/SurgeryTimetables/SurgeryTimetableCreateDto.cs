using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public Guid? SurgeryTimetableId { get; set; }
    }
}