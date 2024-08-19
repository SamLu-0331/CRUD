using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Misars.Foundation.App.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Misars.Foundation.App.Doctors;

public class EfCoreDoctorRepository
    : EfCoreRepository<AppDbContext, Doctor, Guid>,
        IDoctorRepository
{
    public EfCoreDoctorRepository(
        IDbContextProvider<AppDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Doctor> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(doctor => doctor.Name == name);
    }

    public async Task<List<Doctor>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                doctor => doctor.Name.Contains(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}
