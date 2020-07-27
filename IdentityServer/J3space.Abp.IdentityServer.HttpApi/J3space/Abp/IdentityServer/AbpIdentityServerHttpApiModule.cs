using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace J3space.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpIdentityServerApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class AbpIdentityServerHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpIdentityServerHttpApiModule).Assembly);
            });
        }
    }
}