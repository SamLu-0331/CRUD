using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Misars.Foundation.App.Doctors;

public interface IDoctorAppService : IApplicationService
{
    Task<DoctorDto> GetAsync(Guid id);

    Task<PagedResultDto<DoctorDto>> GetListAsync(GetDoctorListDto input);

    Task<DoctorDto> CreateAsync(CreateDoctorDto input);

    Task UpdateAsync(Guid id, UpdateDoctorDto input);

    Task DeleteAsync(Guid id);
}
