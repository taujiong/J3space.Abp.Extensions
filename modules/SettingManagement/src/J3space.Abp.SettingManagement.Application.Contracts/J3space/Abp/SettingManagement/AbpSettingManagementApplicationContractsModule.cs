using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace J3space.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class AbpSettingManagementApplicationContractsModule : AbpModule
    {
    }
}