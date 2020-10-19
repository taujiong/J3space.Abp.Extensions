using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace J3space.Auth.EntityFrameworkCore.DbMigrations
{
    [DependsOn(typeof(AuthEntityFrameworkCoreModule))]
    public class AuthEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AuthMigrationsDbContext>();
        }
    }
}