using J3space.Sample.MongoDb;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace J3space.Sample.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(SampleMongoDbModule),
        typeof(SampleApplicationContractsModule)
    )]
    public class SampleDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}