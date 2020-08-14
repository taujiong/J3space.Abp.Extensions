using J3space.Abp.Account;
using J3space.Abp.IdentityServer;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;

namespace J3space.Sample
{
    [DependsOn(
        typeof(SampleApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityServerHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule)
    )]
    public class SampleHttpApiModule : AbpModule
    {
    }
}