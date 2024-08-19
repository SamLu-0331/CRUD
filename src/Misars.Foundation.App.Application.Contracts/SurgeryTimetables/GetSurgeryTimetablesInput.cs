using Volo.Abp.Application.Dtos;
using System;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class GetSurgeryTimetablesInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public string? BirthDate { get; set; }
        public Guid? DoctorId { get; set; }

        public GetSurgeryTimetablesInput()
        {

        }
    }
}