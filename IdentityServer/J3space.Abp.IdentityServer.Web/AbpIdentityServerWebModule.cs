using J3space.Abp.Account.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace J3space.Abp.IdentityServer.Web
{
    [DependsOn(
        typeof(AbpAccountWebModule),
        typeof(AbpIdentityServerDomainModule)
    )]
    public class AbpIdentityServerWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpIdentityAspNetCoreOptions>(options =>
            {
                options.ConfigureAuthentication = false;
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpIdentityServerWebModule)
                    .Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpIdentityServerWebModule>(
                    "J3space.Abp.IdentityServer.Web");
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityServerMainMenuContributor());
            });

            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }
    }
}
