using Misars.Foundation.App.Shared;
using Misars.Foundation.App.SurgeryTimetables;
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

        public SurgeryTimetablesAppService(ISurgeryTimetableRepository surgeryTimetableRepository, SurgeryTimetableManager surgeryTimetableManager)
        {

            _surgeryTimetableRepository = surgeryTimetableRepository;
            _surgeryTimetableManager = surgeryTimetableManager;

        }

        public virtual async Task<PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>> GetListAsync(GetSurgeryTimetablesInput input)
        {
            var totalCount = await _surgeryTimetableRepository.GetCountAsync(input.FilterText, input.Name, input.BirthDateMin, input.BirthDateMax, input.SurgeryTimetableId);
            var items = await _surgeryTimetableRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Name, input.BirthDateMin, input.BirthDateMax, input.SurgeryTimetableId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<SurgeryTimetableWithNavigationProperties>, List<SurgeryTimetableWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<SurgeryTimetableWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<SurgeryTimetableWithNavigationProperties, SurgeryTimetableWithNavigationPropertiesDto>
                (await _surgeryTimetableRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<SurgeryTimetableDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(await _surgeryTimetableRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetSurgeryTimetableLookupAsync(LookupRequestDto input)
        {
            var query = (await _surgeryTimetableRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Misars.Foundation.App.SurgeryTimetables.SurgeryTimetable>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Misars.Foundation.App.SurgeryTimetables.SurgeryTimetable>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(AppPermissions.SurgeryTimetables.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _surgeryTimetableRepository.DeleteAsync(id);
        }

        [Authorize(AppPermissions.SurgeryTimetables.Create)]
        public virtual async Task<SurgeryTimetableDto> CreateAsync(SurgeryTimetableCreateDto input)
        {

            var surgeryTimetable = await _surgeryTimetableManager.CreateAsync(
            input.SurgeryTimetableId, input.Name, input.BirthDate
            );

            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(surgeryTimetable);
        }

        [Authorize(AppPermissions.SurgeryTimetables.Edit)]
        public virtual async Task<SurgeryTimetableDto> UpdateAsync(Guid id, SurgeryTimetableUpdateDto input)
        {

            var surgeryTimetable = await _surgeryTimetableManager.UpdateAsync(
            id,
            input.SurgeryTimetableId, input.Name, input.BirthDate
            );

            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(surgeryTimetable);
        }
    }
}