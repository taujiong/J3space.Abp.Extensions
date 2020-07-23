using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace J3space.AuthServer.Data
{
    public class NullAuthServerDbSchemaMigrator : IAuthServerDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}