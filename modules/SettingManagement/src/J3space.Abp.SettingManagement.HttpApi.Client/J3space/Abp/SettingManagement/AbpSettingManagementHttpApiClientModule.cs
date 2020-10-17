using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace J3space.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpSettingManagementApplicationContractsModule)
    )]
    public class AbpSettingManagementHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpSettingManagementHttpApiClientModule).Assembly,
                SettingManagementRemoteServiceConstants.RemoteServiceName
            );
        }
    }
}