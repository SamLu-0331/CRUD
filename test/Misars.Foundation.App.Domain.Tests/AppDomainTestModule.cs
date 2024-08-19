using Volo.Abp.Modularity;

namespace Misars.Foundation.App;

[DependsOn(
    typeof(AppDomainModule),
    typeof(AppTestBaseModule)
)]
public class AppDomainTestModule : AbpModule
{

}
