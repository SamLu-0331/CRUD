using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Misars.Foundation.App.SurgeryTimetables;
using Misars.Foundation.App.EntityFrameworkCore;
using Xunit;

namespace Misars.Foundation.App.EntityFrameworkCore.Domains.SurgeryTimetables
{
    public class SurgeryTimetableRepositoryTests : AppEntityFrameworkCoreTestBase
    {
        private readonly ISurgeryTimetableRepository _surgeryTimetableRepository;

        public SurgeryTimetableRepositoryTests()
        {
            _surgeryTimetableRepository = GetRequiredService<ISurgeryTimetableRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _surgeryTimetableRepository.GetListAsync(
                    name: "427e7b15a3d54452adda6adfd2257bbf68031a10bf29417d9dd8b1d5004ec7d29baf95a691b44",
                    birthDate: "aca3fac53f29416a84bc7be978e0f13b7ae8d5c2e53d4f52ac115"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(1);
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _surgeryTimetableRepository.GetCountAsync(
                    name: "b0b9acc958644270897de6f63f2d206ee93bbd35ff504c68ae98c",
                    birthDate: "244489a301e14e4f8b658af38b6768f7e6935f816b9a4261ad06ad622e8a56aa0fcaac23324248c1ba972498d15468bd4a7"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}