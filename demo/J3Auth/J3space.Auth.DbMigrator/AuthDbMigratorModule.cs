using J3space.Auth.EntityFrameworkCore.DbMigrations;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace J3space.Auth.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AuthEntityFrameworkCoreDbMigrationsModule)
    )]
    public class AuthDbMigratorModule : AbpModule
    {
    }
}