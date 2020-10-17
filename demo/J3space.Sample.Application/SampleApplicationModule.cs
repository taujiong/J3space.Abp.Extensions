using J3space.Abp.IdentityServer;
using J3space.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace J3space.Sample
{
    [DependsOn(
        typeof(AbpAccountApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityServerApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(SampleApplicationContractsModule),
        typeof(SampleDomainModule)
    )]
    public class SampleApplicationModule : AbpModule
    {
    }
}