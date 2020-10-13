using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace J3space.Sample.Data
{
    public class NullSampleDbSchemaMigrator : ISampleDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}