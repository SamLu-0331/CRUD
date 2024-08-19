using Misars.Foundation.App.SurgeryTimetables;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetable : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        public virtual DateTime BirthDate { get; set; }
        public Guid? SurgeryTimetableId { get; set; }

        protected SurgeryTimetable()
        {

        }

        public SurgeryTimetable(Guid id, Guid? surgeryTimetableId, string name, DateTime birthDate)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Name = name;
            BirthDate = birthDate;
            SurgeryTimetableId = surgeryTimetableId;
        }

    }
}