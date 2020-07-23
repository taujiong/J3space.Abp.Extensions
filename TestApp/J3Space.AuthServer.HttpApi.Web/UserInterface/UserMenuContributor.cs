using System.Threading.Tasks;
using J3space.Abp.Account.Localization;
using J3space.AuthServer.Localization;
using Volo.Abp.UI.Navigation;

namespace J3space.AuthServer.UserInterface
{
    public class UserMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.User)
            {
                return Task.CompletedTask;
            }

            var localizer = context.GetLocalizer<AuthServerResource>();
            var accountLocalizer = context.GetLocalizer<AbpAccountResource>();

            context.Menu.AddItem(new ApplicationMenuItem(
                "Account.Manage",
                localizer["ManageYourProfile"],
                url: "~/Account/Manage",
                icon: "fa fa-cog",
                order: 1000,
                null));
            context.Menu.AddItem(new ApplicationMenuItem(
                "Account.Logout",
                accountLocalizer["Logout"],
                url: "~/Account/Logout",
                icon: "fa fa-power-off",
                order: int.MaxValue - 1000));

            return Task.CompletedTask;
        }
    }
}
