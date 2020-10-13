using J3space.Abp.IdentityServer;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace J3space.Sample
{
    [DependsOn(
        typeof(SampleDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(SampleApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityServerApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule)
    )]
    public class SampleApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<SampleApplicationModule>(); });
        }
    }
}