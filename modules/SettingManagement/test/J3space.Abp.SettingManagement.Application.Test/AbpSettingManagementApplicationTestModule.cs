using J3space.Abp.SettingManagement.TestBase;
using Volo.Abp.Modularity;

namespace J3space.Abp.SettingManagement.Application.Test
{
    [DependsOn(
        typeof(AbpSettingManagementTestBaseModule),
        typeof(AbpSettingManagementApplicationModule)
    )]
    public class AbpSettingManagementApplicationTestModule : AbpModule
    {
    }
}