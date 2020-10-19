using J3space.Admin.Application.Contracts;
using J3space.Admin.EntityFrameworkCore.DbMigrations;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace J3space.Admin.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AdminApplicationContractsModule),
        typeof(AdminEntityFrameworkCoreDbMigrationsModule)
    )]
    public class AdminDbMigratorModule : AbpModule
    {
    }
}