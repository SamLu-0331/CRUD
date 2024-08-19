using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetableManager : DomainService
    {
        protected ISurgeryTimetableRepository _surgeryTimetableRepository;

        public SurgeryTimetableManager(ISurgeryTimetableRepository surgeryTimetableRepository)
        {
            _surgeryTimetableRepository = surgeryTimetableRepository;
        }

        public virtual async Task<SurgeryTimetable> CreateAsync(
        Guid? surgeryTimetableId, string name, DateTime birthDate)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(birthDate, nameof(birthDate));

            var surgeryTimetable = new SurgeryTimetable(
             GuidGenerator.Create(),
             surgeryTimetableId, name, birthDate
             );

            return await _surgeryTimetableRepository.InsertAsync(surgeryTimetable);
        }

        public virtual async Task<SurgeryTimetable> UpdateAsync(
            Guid id,
            Guid? surgeryTimetableId, string name, DateTime birthDate
        )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(birthDate, nameof(birthDate));

            var surgeryTimetable = await _surgeryTimetableRepository.GetAsync(id);

            surgeryTimetable.SurgeryTimetableId = surgeryTimetableId;
            surgeryTimetable.Name = name;
            surgeryTimetable.BirthDate = birthDate;

            return await _surgeryTimetableRepository.UpdateAsync(surgeryTimetable);
        }

    }
}