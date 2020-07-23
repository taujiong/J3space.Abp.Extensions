using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace J3space.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpAutoMapperModule)
        )]
    public class AbpIdentityServerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpIdentityServerApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<IdentityServerAutoMapperProfile>(validate: true);
            });
        }
    }
}
