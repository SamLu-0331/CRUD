using AutoMapper;
using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Patients;

namespace Misars.Foundation.App.Blazor.Client;

public class AppBlazorAutoMapperProfile : Profile
{
    public AppBlazorAutoMapperProfile()
    {
        CreateMap<PatientDto, CreateUpdatePatientDto>();
        CreateMap<DoctorDto, UpdateDoctorDto>();
        //Define your AutoMapper configuration here for the Blazor project.
    }
}