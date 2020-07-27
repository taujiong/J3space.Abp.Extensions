using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace J3space.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpUiNavigationModule)
    )]
    public class AbpAccountApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountApplicationModule>();
            });

            context.Services.AddAutoMapperObjectMapper<AbpAccountApplicationModule>();
        }
    }
}