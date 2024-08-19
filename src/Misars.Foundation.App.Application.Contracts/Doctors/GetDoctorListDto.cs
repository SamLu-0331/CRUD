using Volo.Abp.Application.Dtos;

namespace Misars.Foundation.App.Doctors;

public class GetDoctorListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
