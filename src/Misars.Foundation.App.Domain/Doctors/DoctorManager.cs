using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Misars.Foundation.App.Doctors;

public class DoctorManager : DomainService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorManager(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<Doctor> CreateAsync(
        string name,
        DateTime birthDate,
        string? shortBio = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingDoctor = await _doctorRepository.FindByNameAsync(name);
        if (existingDoctor != null)
        {
            throw new DoctorAlreadyExistsException(name);
        }

        return new Doctor(
            GuidGenerator.Create(),
            name,
            birthDate,
            shortBio
        );
    }

    public async Task ChangeNameAsync(
        Doctor doctor,
        string newName)
    {
        Check.NotNull(doctor, nameof(doctor));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingDoctor = await _doctorRepository.FindByNameAsync(newName);
        if (existingDoctor != null && existingDoctor.Id != doctor.Id)
        {
            throw new DoctorAlreadyExistsException(newName);
        }

        doctor.ChangeName(newName);
    }
}
