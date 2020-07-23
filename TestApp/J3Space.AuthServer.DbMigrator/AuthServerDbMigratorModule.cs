using J3space.AuthServer;
using J3space.AuthServer.MongoDb;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace J3space.AuthServer.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AuthServerMongoDbModule),
        typeof(AuthServerApplicationContractsModule)
    )]
    public class AuthServerDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
