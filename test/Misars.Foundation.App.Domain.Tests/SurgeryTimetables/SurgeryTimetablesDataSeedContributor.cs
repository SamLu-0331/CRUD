using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Misars.Foundation.App.SurgeryTimetables;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetablesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly ISurgeryTimetableRepository _surgeryTimetableRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SurgeryTimetablesDataSeedContributor(ISurgeryTimetableRepository surgeryTimetableRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _surgeryTimetableRepository = surgeryTimetableRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _surgeryTimetableRepository.InsertAsync(new SurgeryTimetable
            (
                name: "427e7b15a3d54452adda6adfd2257bbf68031a10bf29417d9dd8b1d5004ec7d29baf95a691b44",
                birthDate: "aca3fac53f29416a84bc7be978e0f13b7ae8d5c2e53d4f52ac115"
            ));

            await _surgeryTimetableRepository.InsertAsync(new SurgeryTimetable
            (
                name: "b0b9acc958644270897de6f63f2d206ee93bbd35ff504c68ae98c",
                birthDate: "244489a301e14e4f8b658af38b6768f7e6935f816b9a4261ad06ad622e8a56aa0fcaac23324248c1ba972498d15468bd4a7"
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}