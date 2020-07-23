using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace J3space.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpAccountHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(AbpAccountApplicationContractsModule).Assembly,
                AbpAccountRemoteServiceConstants.RemoteServiceName);
        }
    }
}
