using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Misars.Foundation.App.Patients;

public interface IPatientAppService :
    ICrudAppService< //Defines CRUD methods
        PatientDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdatePatientDto> //Used to create/update a book
{
    Task<ListResultDto<DoctorLookupDto>> GetDoctorLookupAsync();

}
