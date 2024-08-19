using Misars.Foundation.App.Shared;
using Misars.Foundation.App.Doctors;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.SurgeryTimetables;

namespace Misars.Foundation.App.SurgeryTimetables
{
    [RemoteService(IsEnabled = false)]
    [Authorize(AppPermissions.SurgeryTimetables.Default)]
    public class SurgeryTimetablesAppService : AppAppService, ISurgeryTimetablesAppService
    {

        protected ISurgeryTimetableRepository _surgeryTimetableRepository;
        protected SurgeryTimetableManager _surgeryTimetableManager;

        protected IRepository<Doctors.Doctor, Guid> _doctorRepository;

        public SurgeryTimetablesAppService(ISurgeryTimetableRepository surgeryTimetableRepository, SurgeryTimetableManager surgeryTimetableManager, IRepository<Doctors.Doctor, Guid> doctorRepository)
        {

            _surgeryTimetableRepository = surgeryTimetableRepository;
            _surgeryTimetableManager = surgeryTimetableManager; _doctorRepository = doctorRepository;

        }

        public virtual async Task<PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>> GetListAsync(GetSurgeryTimetablesInput input)
        {
            var totalCount = await _surgeryTimetableRepository.GetCountAsync(input.FilterText, input.Name, input.BirthDate, input.DoctorId);
            var items = await _surgeryTimetableRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Name, input.BirthDate, input.DoctorId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<SurgeryTimetableWithNavigationProperties>, List<SurgeryTimetableWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<SurgeryTimetableWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(string id)
        {
            return ObjectMapper.Map<SurgeryTimetableWithNavigationProperties, SurgeryTimetableWithNavigationPropertiesDto>
                (await _surgeryTimetableRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<SurgeryTimetableDto> GetAsync(string id)
        {
            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(await _surgeryTimetableRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetDoctorLookupAsync(LookupRequestDto input)
        {
            var query = (await _doctorRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Doctors.Doctor>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Doctors.Doctor>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(AppPermissions.SurgeryTimetables.Delete)]
        public virtual async Task DeleteAsync(string id)
        {
            await _surgeryTimetableRepository.DeleteAsync(id);
        }

        [Authorize(AppPermissions.SurgeryTimetables.Create)]
        public virtual async Task<SurgeryTimetableDto> CreateAsync(SurgeryTimetableCreateDto input)
        {

            var surgeryTimetable = await _surgeryTimetableManager.CreateAsync(
            input.DoctorIds, input.Name, input.BirthDate
            );

            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(surgeryTimetable);
        }

        [Authorize(AppPermissions.SurgeryTimetables.Edit)]
        public virtual async Task<SurgeryTimetableDto> UpdateAsync(string id, SurgeryTimetableUpdateDto input)
        {

            var surgeryTimetable = await _surgeryTimetableManager.UpdateAsync(
            id,
            input.DoctorIds, input.Name, input.BirthDate
            );

            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(surgeryTimetable);
        }
    }
}