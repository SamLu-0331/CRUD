using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public interface ISurgeryTimetableRepository : IRepository<SurgeryTimetable, string>
    {
        Task<SurgeryTimetableWithNavigationProperties> GetWithNavigationPropertiesAsync(
    string id,
    CancellationToken cancellationToken = default
);

        Task<List<SurgeryTimetableWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? name = null,
            string? birthDate = null,
            Guid? doctorId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<SurgeryTimetable>> GetListAsync(
                    string? filterText = null,
                    string? name = null,
                    string? birthDate = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            string? birthDate = null,
            Guid? doctorId = null,
            CancellationToken cancellationToken = default);
    }
}