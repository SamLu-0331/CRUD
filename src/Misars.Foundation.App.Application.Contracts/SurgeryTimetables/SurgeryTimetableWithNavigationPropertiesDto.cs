using Misars.Foundation.App.SurgeryTimetables;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableWithNavigationPropertiesDto
    {
        public SurgeryTimetableDto SurgeryTimetable { get; set; } = null!;

        public SurgeryTimetableDto SurgeryTimetable1 { get; set; } = null!;

    }
}