using Misars.Foundation.App.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Misars.Foundation.App.Blazor.Client;

public abstract class AppComponentBase : AbpComponentBase
{
    protected AppComponentBase()
    {
        LocalizationResource = typeof(AppResource);
    }
}
