using J3space.Abp.IdentityServer;
using J3space.Abp.SettingManagement;
using J3space.Admin.Domain.Shared;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace J3space.Admin.Application.Contracts
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpIdentityServerApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AdminDomainSharedModule)
    )]
    public class AdminApplicationContractsModule : AbpModule
    {
    }
}