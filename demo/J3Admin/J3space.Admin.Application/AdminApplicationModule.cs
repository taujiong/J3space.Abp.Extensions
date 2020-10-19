using J3space.Abp.IdentityServer;
using J3space.Abp.SettingManagement;
using J3space.Admin.Application.Contracts;
using J3space.Admin.Domain;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace J3space.Admin.Application
{
    [DependsOn(
        typeof(AbpAccountApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityServerApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(AdminApplicationContractsModule),
        typeof(AdminDomainModule)
    )]
    public class AdminApplicationModule : AbpModule
    {
    }
}