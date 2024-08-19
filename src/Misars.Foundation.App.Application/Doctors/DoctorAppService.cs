using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Misars.Foundation.App.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.Doctors;

[Authorize(AppPermissions.Doctors.Default)]
public class DoctorAppService : AppAppService, IDoctorAppService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly DoctorManager _doctorManager;

    public DoctorAppService(
        IDoctorRepository doctorRepository,
        DoctorManager doctorManager)
    {
        _doctorRepository = doctorRepository;
        _doctorManager = doctorManager;
    }
    public async Task<DoctorDto> GetAsync(Guid id)
{
    var doctor = await _doctorRepository.GetAsync(id);
    return ObjectMapper.Map<Doctor, DoctorDto>(doctor);
}
public async Task<PagedResultDto<DoctorDto>> GetListAsync(GetDoctorListDto input)
{
    if (input.Sorting.IsNullOrWhiteSpace())
    {
        input.Sorting = nameof(Doctor.Name);
    }

    var doctors = await _doctorRepository.GetListAsync(
        input.SkipCount,
        input.MaxResultCount,
        input.Sorting,
        input.Filter
    );

    var totalCount = input.Filter == null
        ? await _doctorRepository.CountAsync()
        : await _doctorRepository.CountAsync(
            doctor => doctor.Name.Contains(input.Filter));
    return new PagedResultDto<DoctorDto>(
        totalCount,
        ObjectMapper.Map<List<Doctor>, List<DoctorDto>>(doctors)
    );
}
[Authorize(AppPermissions.Doctors.Create)]
public async Task<DoctorDto> CreateAsync(CreateDoctorDto input)
{
    var doctor = await _doctorManager.CreateAsync(
        input.Name,
        input.BirthDate,
        input.ShortBio
    );

    await _doctorRepository.InsertAsync(doctor);

    return ObjectMapper.Map<Doctor, DoctorDto>(doctor);
}
[Authorize(AppPermissions.Doctors.Edit)]
public async Task UpdateAsync(Guid id, UpdateDoctorDto input)
{
    var doctor = await _doctorRepository.GetAsync(id);

    if (doctor.Name != input.Name)
    {
        await _doctorManager.ChangeNameAsync(doctor, input.Name);
    }

    doctor.BirthDate = input.BirthDate;
    doctor.ShortBio = input.ShortBio;

    await _doctorRepository.UpdateAsync(doctor);
}
[Authorize(AppPermissions.Doctors.Delete)]
public async Task DeleteAsync(Guid id)
{
    await _doctorRepository.DeleteAsync(id);
}

    //...SERVICE METHODS WILL COME HERE...
}
