using J3space.Abp.Account;
using J3space.Abp.IdentityServer;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace J3space.AuthServer
{
    [DependsOn(
        typeof(AuthServerDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(AuthServerApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityServerApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule)
    )]
    public class AuthServerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<AuthServerApplicationModule>(); });
        }
    }
}