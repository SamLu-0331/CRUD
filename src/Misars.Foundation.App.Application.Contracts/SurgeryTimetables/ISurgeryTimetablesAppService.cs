using Misars.Foundation.App.Shared;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public interface ISurgeryTimetablesAppService : IApplicationService
    {

        Task<PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>> GetListAsync(GetSurgeryTimetablesInput input);

        Task<SurgeryTimetableWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<SurgeryTimetableDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetDoctorLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<SurgeryTimetableDto> CreateAsync(SurgeryTimetableCreateDto input);

        Task<SurgeryTimetableDto> UpdateAsync(Guid id, SurgeryTimetableUpdateDto input);
    }
}