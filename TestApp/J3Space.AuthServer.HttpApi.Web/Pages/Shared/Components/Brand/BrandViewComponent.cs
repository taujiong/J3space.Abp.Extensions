using System.Threading.Tasks;
using J3space.AuthServer.Settings;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace J3space.AuthServer.Pages.Shared.Components.Brand
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly ISettingProvider _settingProvider;

        public BrandViewComponent(ISettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brand = new BrandViewModel
            {
                AppName = await _settingProvider.GetOrNullAsync(AuthServerSettings.App.Name),
                AppLogoUrl = await _settingProvider.GetOrNullAsync(AuthServerSettings.App.LogoUrl)
            };
            return View("~/Pages/Shared/Components/Brand/Default.cshtml", brand);
        }
    }
}
