using Misars.Foundation.App.Doctors;
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
        protected IRepository<Doctor, Guid> _doctorRepository;

        public SurgeryTimetableManager(ISurgeryTimetableRepository surgeryTimetableRepository,
        IRepository<Doctor, Guid> doctorRepository)
        {
            _surgeryTimetableRepository = surgeryTimetableRepository;
            _doctorRepository = doctorRepository;
        }

        public virtual async Task<SurgeryTimetable> CreateAsync(
        List<Guid> doctorIds,
        string name, string birthDate)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(birthDate, nameof(birthDate));

            var surgeryTimetable = new SurgeryTimetable(

             name, birthDate
             );

            await SetDoctorsAsync(surgeryTimetable, doctorIds);

            return await _surgeryTimetableRepository.InsertAsync(surgeryTimetable);
        }

        public virtual async Task<SurgeryTimetable> UpdateAsync(
            string id,
            List<Guid> doctorIds,
        string name, string birthDate
        )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(birthDate, nameof(birthDate));

            var queryable = await _surgeryTimetableRepository.WithDetailsAsync(x => x.Doctors);
            var query = queryable.Where(x => x.Id == id);

            var surgeryTimetable = await AsyncExecuter.FirstOrDefaultAsync(query);

            surgeryTimetable.Name = name;
            surgeryTimetable.BirthDate = birthDate;

            await SetDoctorsAsync(surgeryTimetable, doctorIds);

            return await _surgeryTimetableRepository.UpdateAsync(surgeryTimetable);
        }

        private async Task SetDoctorsAsync(SurgeryTimetable surgeryTimetable, List<Guid> doctorIds)
        {
            if (doctorIds == null || !doctorIds.Any())
            {
                surgeryTimetable.RemoveAllDoctors();
                return;
            }

            var query = (await _doctorRepository.GetQueryableAsync())
                .Where(x => doctorIds.Contains(x.Id))
                .Select(x => x.Id);

            var doctorIdsInDb = await AsyncExecuter.ToListAsync(query);
            if (!doctorIdsInDb.Any())
            {
                return;
            }

            surgeryTimetable.RemoveAllDoctorsExceptGivenIds(doctorIdsInDb);

            foreach (var doctorId in doctorIdsInDb)
            {
                surgeryTimetable.AddDoctor(doctorId);
            }
        }

    }
}