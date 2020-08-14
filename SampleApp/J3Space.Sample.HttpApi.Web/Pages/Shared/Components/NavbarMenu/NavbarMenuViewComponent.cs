using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.UI.Navigation;

namespace J3space.Sample.Pages.Shared.Components.NavbarMenu
{
    public class NavbarMenuViewComponent : ViewComponent
    {
        private readonly IMenuManager _menuManager;

        public NavbarMenuViewComponent(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = await _menuManager.GetAsync(StandardMenus.Main);
            return View("~/Pages/Shared/Components/NavbarMenu/Default.cshtml", menu);
        }
    }
}