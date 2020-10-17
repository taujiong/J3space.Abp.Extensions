using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace J3space.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
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