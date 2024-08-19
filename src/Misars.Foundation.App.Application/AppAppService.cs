using Misars.Foundation.App.Localization;
using Volo.Abp.Application.Services;

namespace Misars.Foundation.App;

/* Inherit your application services from this class.
 */
public abstract class AppAppService : ApplicationService
{
    protected AppAppService()
    {
        LocalizationResource = typeof(AppResource);
    }
}
