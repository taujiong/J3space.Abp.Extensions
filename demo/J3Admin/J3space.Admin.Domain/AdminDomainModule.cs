using J3space.Admin.Domain.Shared;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;

namespace J3space.Admin.Domain
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AdminDomainSharedModule)
    )]
    public class AdminDomainModule : AbpModule
    {
    }
}