using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace J3space.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpIdentityServerApplicationContractsModule)
    )]
    public class AbpIdentityServerHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AbpIdentityServerApplicationContractsModule).Assembly,
                IdentityServerRemoteServiceConstants.RemoteServiceName
            );
        }
    }
}