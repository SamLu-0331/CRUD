using Misars.Foundation.App.Samples;
using Xunit;

namespace Misars.Foundation.App.EntityFrameworkCore.Domains;

[Collection(AppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AppEntityFrameworkCoreTestModule>
{

}
