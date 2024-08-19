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
    public class SurgeryTimetable : FullAuditedAggregateRoot<string>
    {
        [NotNull]
        public virtual string Name { get; set; }

        [NotNull]
        public virtual string BirthDate { get; set; }

        public ICollection<SurgeryTimetableDoctor> Doctors { get; private set; }

        protected SurgeryTimetable()
        {

        }

        public SurgeryTimetable(string name, string birthDate)
        {

            Check.NotNull(name, nameof(name));
            Check.NotNull(birthDate, nameof(birthDate));
            Name = name;
            BirthDate = birthDate;
            Doctors = new Collection<SurgeryTimetableDoctor>();
        }
        public virtual void AddDoctor(Guid doctorId)
        {
            Check.NotNull(doctorId, nameof(doctorId));

            if (IsInDoctors(doctorId))
            {
                return;
            }

            Doctors.Add(new SurgeryTimetableDoctor(Id, doctorId));
        }

        public virtual void RemoveDoctor(Guid doctorId)
        {
            Check.NotNull(doctorId, nameof(doctorId));

            if (!IsInDoctors(doctorId))
            {
                return;
            }

            Doctors.RemoveAll(x => x.DoctorId == doctorId);
        }

        public virtual void RemoveAllDoctorsExceptGivenIds(List<Guid> doctorIds)
        {
            Check.NotNullOrEmpty(doctorIds, nameof(doctorIds));

            Doctors.RemoveAll(x => !doctorIds.Contains(x.DoctorId));
        }

        public virtual void RemoveAllDoctors()
        {
            Doctors.RemoveAll(x => x.SurgeryTimetableId == Id);
        }

        private bool IsInDoctors(Guid doctorId)
        {
            return Doctors.Any(x => x.DoctorId == doctorId);
        }
    }
}