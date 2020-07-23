using Volo.Abp.Application;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace J3space.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpIdentityServerDomainSharedModule),
        typeof(AbpIdentityServerDomainModule)
        )]
    public class AbpIdentityServerApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpIdentityServerApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpIdentityServerResource>()
                    .AddVirtualJson("/J3space/Abp/IdentityServer/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("J3space.IdentityServer", typeof(AbpIdentityServerResource));
            });
        }
    }
}
