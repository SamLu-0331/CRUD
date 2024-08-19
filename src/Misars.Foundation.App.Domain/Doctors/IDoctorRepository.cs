using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.Doctors;

public interface IDoctorRepository : IRepository<Doctor, Guid>
{
    Task<Doctor> FindByNameAsync(string name);

    Task<List<Doctor>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null
    );
}
