using System.Threading.Tasks;
using J3space.Abp.IdentityServer;
using J3space.AuthServer.Localization;
using Volo.Abp.UI.Navigation;

namespace J3space.AuthServer.UserInterface
{
    public class MainMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return;
            }

            var localizer = context.GetLocalizer<AuthServerResource>();
            var hasClientPermission = await context.IsGrantedAsync(IdentityServerPermissions.Clients.Default);
            var hasApiResourcePermission =
                await context.IsGrantedAsync(IdentityServerPermissions.ApiResources.Default);
            var hasIdentityResourcePermission =
                await context.IsGrantedAsync(IdentityServerPermissions.IdentityResources.Default);

            if (hasClientPermission || hasApiResourcePermission || hasIdentityResourcePermission)
            {
                var idsMenu = new ApplicationMenuItem(
                    "IdentityServer",
                    localizer["Permission:IdentityServerManagement"]
                );

                if (hasClientPermission)
                {
                    idsMenu.AddItem(new ApplicationMenuItem(
                        "IdentityServer.Client",
                        localizer["Permission:ClientManagement"],
                        url: "~/Ids/Client"
                    ));
                }

                if (hasApiResourcePermission)
                {
                    idsMenu.AddItem(new ApplicationMenuItem(
                        "IdentityServer.ApiResource",
                        localizer["Permission:ApiResourcesManagement"],
                        url: "~/Ids/ApiResource"
                    ));
                }

                if (hasIdentityResourcePermission)
                {
                    idsMenu.AddItem(new ApplicationMenuItem(
                        "IdentityServer.IdentityResource",
                        localizer["Permission:IdentityResourcesManagement"],
                        url: "~/Ids/IdentityResource"
                    ));
                }

                context.Menu.AddItem(idsMenu);
            }
        }
    }
}
