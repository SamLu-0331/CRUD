using System.Threading.Tasks;

namespace Misars.Foundation.App.Data;

public interface IAppDbSchemaMigrator
{
    Task MigrateAsync();
}
