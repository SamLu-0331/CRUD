using Misars.Foundation.App.SurgeryTimetables;
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
    public class EfCoreSurgeryTimetableRepository : EfCoreRepository<AppDbContext, SurgeryTimetable, Guid>, ISurgeryTimetableRepository
    {
        public EfCoreSurgeryTimetableRepository(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<SurgeryTimetableWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(surgeryTimetable => new SurgeryTimetableWithNavigationProperties
                {
                    SurgeryTimetable = surgeryTimetable,
                    SurgeryTimetable1 = dbContext.Set<SurgeryTimetable>().FirstOrDefault(c => c.Id == surgeryTimetable.SurgeryTimetableId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<SurgeryTimetableWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? name = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? surgeryTimetableId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, birthDateMin, birthDateMax, surgeryTimetableId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SurgeryTimetableConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<SurgeryTimetableWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from surgeryTimetable in (await GetDbSetAsync())
                   join surgeryTimetable1 in (await GetDbContextAsync()).Set<SurgeryTimetable>() on surgeryTimetable.SurgeryTimetableId equals surgeryTimetable1.Id into surgeryTimetables1
                   from surgeryTimetable1 in surgeryTimetables1.DefaultIfEmpty()
                   select new SurgeryTimetableWithNavigationProperties
                   {
                       SurgeryTimetable = surgeryTimetable,
                       SurgeryTimetable1 = surgeryTimetable1
                   };
        }

        protected virtual IQueryable<SurgeryTimetableWithNavigationProperties> ApplyFilter(
            IQueryable<SurgeryTimetableWithNavigationProperties> query,
            string? filterText,
            string? name = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? surgeryTimetableId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.SurgeryTimetable.Name!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.SurgeryTimetable.Name.Contains(name))
                    .WhereIf(birthDateMin.HasValue, e => e.SurgeryTimetable.BirthDate >= birthDateMin!.Value)
                    .WhereIf(birthDateMax.HasValue, e => e.SurgeryTimetable.BirthDate <= birthDateMax!.Value)
                    .WhereIf(surgeryTimetableId != null && surgeryTimetableId != Guid.Empty, e => e.SurgeryTimetable1 != null && e.SurgeryTimetable1.Id == surgeryTimetableId);
        }

        public virtual async Task<List<SurgeryTimetable>> GetListAsync(
            string? filterText = null,
            string? name = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, birthDateMin, birthDateMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SurgeryTimetableConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? surgeryTimetableId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, birthDateMin, birthDateMax, surgeryTimetableId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<SurgeryTimetable> ApplyFilter(
            IQueryable<SurgeryTimetable> query,
            string? filterText = null,
            string? name = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(birthDateMin.HasValue, e => e.BirthDate >= birthDateMin!.Value)
                    .WhereIf(birthDateMax.HasValue, e => e.BirthDate <= birthDateMax!.Value);
        }
    }
}