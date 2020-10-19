using J3space.Abp.IdentityServer;
using J3space.Abp.SettingManagement;
using J3space.Admin.Application.Contracts;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;

namespace J3space.Admin.HttpApi
{
    [DependsOn(
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AdminApplicationContractsModule)
    )]
    public class AdminHttpApiModule : AbpModule
    {
    }
}