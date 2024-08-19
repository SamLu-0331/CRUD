using AutoMapper;
using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Patients;

namespace Misars.Foundation.App;

public class AppApplicationAutoMapperProfile : Profile
{
    public AppApplicationAutoMapperProfile()
    {
        CreateMap<Patient, PatientDto>();
        CreateMap<CreateUpdatePatientDto, Patient>();
        CreateMap<Doctor, DoctorDto>();
        CreateMap<Doctor, DoctorLookupDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
