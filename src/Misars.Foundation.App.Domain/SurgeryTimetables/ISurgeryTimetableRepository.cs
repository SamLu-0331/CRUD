using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public interface ISurgeryTimetableRepository : IRepository<SurgeryTimetable, Guid>
    {
        Task<SurgeryTimetableWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<SurgeryTimetableWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? name = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? surgeryTimetableId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<SurgeryTimetable>> GetListAsync(
                    string? filterText = null,
                    string? name = null,
                    DateTime? birthDateMin = null,
                    DateTime? birthDateMax = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            Guid? surgeryTimetableId = null,
            CancellationToken cancellationToken = default);
    }
}