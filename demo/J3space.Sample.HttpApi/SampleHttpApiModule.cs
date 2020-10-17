using J3space.Abp.IdentityServer;
using J3space.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;

namespace J3space.Sample
{
    [DependsOn(
        typeof(AbpAccountHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(SampleApplicationContractsModule)
    )]
    public class SampleHttpApiModule : AbpModule
    {
    }
}