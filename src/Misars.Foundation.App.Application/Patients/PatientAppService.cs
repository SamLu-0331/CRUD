using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.Patients;

[Authorize(AppPermissions.Patients.Default)]
public class PatientAppService :
    CrudAppService<
        Patient, //The patient entity
        PatientDto, //Used to show patients
        Guid, //Primary key of the patient entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdatePatientDto>, //Used to create/update a patient
    IPatientAppService //implement the IpatientAppService
{
    private readonly IDoctorRepository _doctorRepository;

    public PatientAppService(
        IRepository<Patient, Guid> repository,
        IDoctorRepository doctorRepository)
        : base(repository)
    {
        _doctorRepository = doctorRepository;
        GetPolicyName = AppPermissions.Patients.Default;
        GetListPolicyName = AppPermissions.Patients.Default;
        CreatePolicyName = AppPermissions.Patients.Create;
        UpdatePolicyName = AppPermissions.Patients.Edit;
        DeletePolicyName = AppPermissions.Patients.Delete;
    }

    public override async Task<PatientDto> GetAsync(Guid id)
    {
        //Get the IQueryable<patient> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join patients and Doctors
        var query = from patient in queryable
            join doctor in await _doctorRepository.GetQueryableAsync() on patient.DoctorId equals doctor.Id
            where patient.Id == id
            select new { patient, doctor };

        //Execute the query and get the patient with Doctor
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Patient), id);
        }

        var patientDto = ObjectMapper.Map<Patient, PatientDto>(queryResult.patient);
        patientDto.DoctorName = queryResult.doctor.Name;
        return patientDto;
    }

    public override async Task<PagedResultDto<PatientDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<patient> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join patients and Doctors
        var query = from patient in queryable
            join doctor in await _doctorRepository.GetQueryableAsync() on patient.DoctorId equals doctor.Id
            select new {patient, doctor};

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of patientDto objects
        var patientDtos = queryResult.Select(x =>
        {
            var patientDto = ObjectMapper.Map<Patient, PatientDto>(x.patient);
            patientDto.DoctorName = x.doctor.Name;
            return patientDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<PatientDto>(
            totalCount,
            patientDtos
        );
    }

    public async Task<ListResultDto<DoctorLookupDto>> GetDoctorLookupAsync()
    {
        var doctors = await _doctorRepository.GetListAsync();

        return new ListResultDto<DoctorLookupDto>(
            ObjectMapper.Map<List<Doctor>, List<DoctorLookupDto>>(doctors)
        );
    }

    private static string NormalizeSorting(string sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"patient.{nameof(Patient.Name)}";
        }

        if (sorting.Contains("doctorName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "doctorName",
                "doctor.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"patient.{sorting}";
    }
}
