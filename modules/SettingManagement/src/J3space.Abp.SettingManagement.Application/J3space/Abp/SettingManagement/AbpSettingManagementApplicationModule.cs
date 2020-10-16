using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace J3space.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpSettingManagementApplicationContractsModule))]
    public class AbpSettingManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpSettingManagementApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpSettingManagementAutoMapperProfile>(true);
            });
        }
    }
}