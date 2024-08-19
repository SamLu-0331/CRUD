using Xunit;

namespace Misars.Foundation.App.EntityFrameworkCore;

[CollectionDefinition(AppTestConsts.CollectionDefinitionName)]
public class AppEntityFrameworkCoreCollection : ICollectionFixture<AppEntityFrameworkCoreFixture>
{

}
