using Misars.Foundation.App.Doctors;

using System;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public  class SurgeryTimetableWithNavigationProperties
    {
        public SurgeryTimetable SurgeryTimetable { get; set; } = null!;

        

        public List<Doctor> Doctors { get; set; } = null!;
        
    }
}