using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace J3space.Auth.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule)
    )]
    public class AuthEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AuthDbContext>(options => { options.AddDefaultRepositories(true); });

            Configure<AbpDbContextOptions>(options => { options.UseMySQL(); });
        }
    }
}