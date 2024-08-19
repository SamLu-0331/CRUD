using Volo.Abp.Modularity;

namespace Misars.Foundation.App;

[DependsOn(
    typeof(AppApplicationModule),
    typeof(AppDomainTestModule)
)]
public class AppApplicationTestModule : AbpModule
{

}
