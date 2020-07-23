using System.Threading.Tasks;
using J3space.Abp.Account.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.UI.Navigation;

namespace J3space.Abp.Account.Web
{
    public class AbpAccountUserMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.User) return Task.CompletedTask;

            var uiResource = context.GetLocalizer<AbpUiResource>();
            var accountResource = context.GetLocalizer<AbpAccountResource>();

            context.Menu.AddItem(new ApplicationMenuItem(
                AbpAccountMenuName.Manage,
                accountResource["ManageYourProfile"],
                "~/Account/Manage",
                "fa fa-cog"));
            context.Menu.AddItem(new ApplicationMenuItem(
                AbpAccountMenuName.Logout,
                uiResource["Logout"],
                "~/Account/Logout",
                "fa fa-power-off",
                int.MaxValue - 1000));

            return Task.CompletedTask;
        }
    }
}