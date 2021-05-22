using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace J3space.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpLocalizationModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class AbpSettingManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpSettingManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpSettingManagementResource>()
                    .AddVirtualJson("/J3space/Abp/SettingManagement/Localization/Resources");
            });
        }
    }
}