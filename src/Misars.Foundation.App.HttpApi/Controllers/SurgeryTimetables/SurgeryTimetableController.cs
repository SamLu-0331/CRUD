using Misars.Foundation.App.Shared;
using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.SurgeryTimetables;

namespace Misars.Foundation.App.Controllers.SurgeryTimetables
{
    [RemoteService]
    [Area("app")]
    [ControllerName("SurgeryTimetable")]
    [Route("api/app/surgery-timetables")]

    public class SurgeryTimetableController : AbpController, ISurgeryTimetablesAppService
    {
        protected ISurgeryTimetablesAppService _surgeryTimetablesAppService;

        public SurgeryTimetableController(ISurgeryTimetablesAppService surgeryTimetablesAppService)
        {
            _surgeryTimetablesAppService = surgeryTimetablesAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>> GetListAsync(GetSurgeryTimetablesInput input)
        {
            return _surgeryTimetablesAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<SurgeryTimetableWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(string id)
        {
            return _surgeryTimetablesAppService.GetWithNavigationPropertiesAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<SurgeryTimetableDto> GetAsync(string id)
        {
            return _surgeryTimetablesAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("doctor-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetDoctorLookupAsync(LookupRequestDto input)
        {
            return _surgeryTimetablesAppService.GetDoctorLookupAsync(input);
        }

        [HttpPost]
        public virtual Task<SurgeryTimetableDto> CreateAsync(SurgeryTimetableCreateDto input)
        {
            return _surgeryTimetablesAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<SurgeryTimetableDto> UpdateAsync(string id, SurgeryTimetableUpdateDto input)
        {
            return _surgeryTimetablesAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return _surgeryTimetablesAppService.DeleteAsync(id);
        }
    }
}