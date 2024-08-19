using Misars.Foundation.App.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Misars.Foundation.App.EntityFrameworkCore;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class EfCoreSurgeryTimetableRepository : EfCoreRepository<AppDbContext, SurgeryTimetable, string>, ISurgeryTimetableRepository
    {
        public EfCoreSurgeryTimetableRepository(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<SurgeryTimetableWithNavigationProperties> GetWithNavigationPropertiesAsync(string id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.Doctors)
                .Select(surgeryTimetable => new SurgeryTimetableWithNavigationProperties
                {
                    SurgeryTimetable = surgeryTimetable,
                    Doctors = (from surgeryTimetableDoctors in surgeryTimetable.Doctors
                               join _doctor in dbContext.Set<Doctor>() on surgeryTimetableDoctors.DoctorId equals _doctor.Id
                               select _doctor).ToList()
                }).FirstOrDefault();
        }

        public virtual async Task<List<SurgeryTimetableWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? name = null,
            string? birthDate = null,
            Guid? doctorId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, birthDate, doctorId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SurgeryTimetableConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<SurgeryTimetableWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from surgeryTimetable in (await GetDbSetAsync())

                   select new SurgeryTimetableWithNavigationProperties
                   {
                       SurgeryTimetable = surgeryTimetable,
                       Doctors = new List<Doctor>()
                   };
        }

        protected virtual IQueryable<SurgeryTimetableWithNavigationProperties> ApplyFilter(
            IQueryable<SurgeryTimetableWithNavigationProperties> query,
            string? filterText,
            string? name = null,
            string? birthDate = null,
            Guid? doctorId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.SurgeryTimetable.Name!.Contains(filterText!) || e.SurgeryTimetable.BirthDate!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.SurgeryTimetable.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(birthDate), e => e.SurgeryTimetable.BirthDate.Contains(birthDate))
                    .WhereIf(doctorId != null && doctorId != Guid.Empty, e => e.SurgeryTimetable.Doctors.Any(x => x.DoctorId == doctorId));
        }

        public virtual async Task<List<SurgeryTimetable>> GetListAsync(
            string? filterText = null,
            string? name = null,
            string? birthDate = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, birthDate);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SurgeryTimetableConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            string? birthDate = null,
            Guid? doctorId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, birthDate, doctorId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<SurgeryTimetable> ApplyFilter(
            IQueryable<SurgeryTimetable> query,
            string? filterText = null,
            string? name = null,
            string? birthDate = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name!.Contains(filterText!) || e.BirthDate!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(birthDate), e => e.BirthDate.Contains(birthDate));
        }
    }
}