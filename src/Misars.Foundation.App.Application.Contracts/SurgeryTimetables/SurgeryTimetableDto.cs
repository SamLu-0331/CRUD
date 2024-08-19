using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public Guid? SurgeryTimetableId { get; set; }

    }
}