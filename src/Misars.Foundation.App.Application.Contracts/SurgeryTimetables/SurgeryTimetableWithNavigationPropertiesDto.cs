using Misars.Foundation.App.Doctors;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableWithNavigationPropertiesDto
    {
        public SurgeryTimetableDto SurgeryTimetable { get; set; } = null!;

        public List<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();

    }
}