using System;
using System.Threading.Tasks;
using Misars.Foundation.App.Patients;
using Misars.Foundation.App.Doctors;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App;

public class AppDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Patient, Guid> _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly DoctorManager _doctorManager;

    public AppDataSeederContributor(
        IRepository<Patient, Guid> patientRepository,
        IDoctorRepository doctorRepository,
        DoctorManager doctorManager)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _doctorManager = doctorManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _patientRepository.GetCountAsync() > 0)
        {
            return;
        }

        var orwell = await _doctorRepository.InsertAsync(
            await _doctorManager.CreateAsync(
                "George Orwell",
                new DateTime(1903, 06, 25),
                "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
            )
        );

        var douglas = await _doctorRepository.InsertAsync(
            await _doctorManager.CreateAsync(
                "Douglas Adams",
                new DateTime(1952, 03, 11),
                "Douglas Adams was an English doctor, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
            )
        );

        await _patientRepository.InsertAsync(
            new Patient
            {
                DoctorId = orwell.Id, // SET THE AUTHOR
                Name = "1984",
                Type = PatientType.Dystopia,
                PublishDate = new DateTime(1949, 6, 8),
                Price = 19.84f
            },
            autoSave: true
        );

        await _patientRepository.InsertAsync(
            new Patient
            {
                DoctorId = douglas.Id, // SET THE AUTHOR
                Name = "The Hitchhiker's Guide to the Galaxy",
                Type = PatientType.ScienceFiction,
                PublishDate = new DateTime(1995, 9, 27),
                Price = 42.0f
            },
            autoSave: true
        );
    }
}
