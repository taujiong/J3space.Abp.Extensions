using Volo.Abp.Modularity;

namespace J3space.Abp.SettingManagement
{
    [DependsOn(typeof(AbpSettingManagementApplicationContractsModule))]
    public class AbpSettingManagementApplicationModule : AbpModule
    {
    }
}