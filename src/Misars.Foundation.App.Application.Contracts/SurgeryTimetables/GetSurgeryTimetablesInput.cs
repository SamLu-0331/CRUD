using Volo.Abp.Application.Dtos;
using System;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class GetSurgeryTimetablesInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public Guid? SurgeryTimetableId { get; set; }

        public GetSurgeryTimetablesInput()
        {

        }
    }
}