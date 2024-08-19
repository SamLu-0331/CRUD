using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableDto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; } = null!;
        public string BirthDate { get; set; } = null!;

    }
}