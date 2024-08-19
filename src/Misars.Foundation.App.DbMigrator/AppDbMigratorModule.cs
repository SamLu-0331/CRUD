using Misars.Foundation.App.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Misars.Foundation.App.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AppEntityFrameworkCoreModule),
    typeof(AppApplicationContractsModule)
)]
public class AppDbMigratorModule : AbpModule
{
}
