using System.Threading.Tasks;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.UI.Navigation;

namespace J3space.Abp.IdentityServer
{
    public class AbpIdentityServerMainMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main) return;

            var localizer = context.GetLocalizer<AbpIdentityServerResource>();
            var hasClientPermission =
                await context.IsGrantedAsync(IdentityServerPermissions.Client.Default);
            var hasApiResourcePermission =
                await context.IsGrantedAsync(IdentityServerPermissions.ApiResource.Default);
            var hasIdentityResourcePermission =
                await context.IsGrantedAsync(IdentityServerPermissions.IdentityResource.Default);

            if (hasClientPermission || hasApiResourcePermission || hasIdentityResourcePermission)
            {
                var idsMenu = new ApplicationMenuItem(
                    AbpIdentityServerMenuNames.GroupName,
                    localizer["Permission:IdentityServerManagement"]
                );

                if (hasClientPermission)
                    idsMenu.AddItem(new ApplicationMenuItem(
                        AbpIdentityServerMenuNames.Client,
                        localizer["Permission:ClientManagement"],
                        "~/Ids/Client"
                    ));

                if (hasApiResourcePermission)
                    idsMenu.AddItem(new ApplicationMenuItem(
                        AbpIdentityServerMenuNames.ApiResource,
                        localizer["Permission:ApiResourceManagement"],
                        "~/Ids/ApiResource"
                    ));

                if (hasIdentityResourcePermission)
                    idsMenu.AddItem(new ApplicationMenuItem(
                        AbpIdentityServerMenuNames.IdentityResource,
                        localizer["Permission:IdentityResourceManagement"],
                        "~/Ids/IdentityResource"
                    ));

                context.Menu.AddItem(idsMenu);
            }
        }
    }
}