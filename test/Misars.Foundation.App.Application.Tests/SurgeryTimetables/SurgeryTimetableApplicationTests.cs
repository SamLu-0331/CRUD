using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetablesAppServiceTests<TStartupModule> : AppApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly ISurgeryTimetablesAppService _surgeryTimetablesAppService;
        private readonly IRepository<SurgeryTimetable, string> _surgeryTimetableRepository;

        public SurgeryTimetablesAppServiceTests()
        {
            _surgeryTimetablesAppService = GetRequiredService<ISurgeryTimetablesAppService>();
            _surgeryTimetableRepository = GetRequiredService<IRepository<SurgeryTimetable, string>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _surgeryTimetablesAppService.GetListAsync(new GetSurgeryTimetablesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.SurgeryTimetable.Id == 1).ShouldBe(true);
            result.Items.Any(x => x.SurgeryTimetable.Id == 2).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _surgeryTimetablesAppService.GetAsync(1);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new SurgeryTimetableCreateDto
            {
                Name = "d589a9bb8555487b917c11a63ded6267d",
                BirthDate = "7b7dcb7da5194c47bb81"
            };

            // Act
            var serviceResult = await _surgeryTimetablesAppService.CreateAsync(input);

            // Assert
            var result = await _surgeryTimetableRepository.FindAsync(c => c.Name == serviceResult.Name);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("d589a9bb8555487b917c11a63ded6267d");
            result.BirthDate.ShouldBe("7b7dcb7da5194c47bb81");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new SurgeryTimetableUpdateDto()
            {
                Name = "b50594a0c4e24dff9f117af0dc214fb0f685fcd266f",
                BirthDate = "2d49c7611de6407a9609663ea1af188789350a1c3ddd41b181c06d43a4802f1026e9de4fb25"
            };

            // Act
            var serviceResult = await _surgeryTimetablesAppService.UpdateAsync(1, input);

            // Assert
            var result = await _surgeryTimetableRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("b50594a0c4e24dff9f117af0dc214fb0f685fcd266f");
            result.BirthDate.ShouldBe("2d49c7611de6407a9609663ea1af188789350a1c3ddd41b181c06d43a4802f1026e9de4fb25");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _surgeryTimetablesAppService.DeleteAsync(1);

            // Assert
            var result = await _surgeryTimetableRepository.FindAsync(c => c.Id == 1);

            result.ShouldBeNull();
        }
    }
}