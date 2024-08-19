using Volo.Abp.Modularity;

namespace Misars.Foundation.App;

public abstract class AppApplicationTestBase<TStartupModule> : AppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
