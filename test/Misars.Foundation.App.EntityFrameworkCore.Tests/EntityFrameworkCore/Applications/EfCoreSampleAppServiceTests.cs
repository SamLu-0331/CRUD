using Misars.Foundation.App.Samples;
using Xunit;

namespace Misars.Foundation.App.EntityFrameworkCore.Applications;

[Collection(AppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AppEntityFrameworkCoreTestModule>
{

}
