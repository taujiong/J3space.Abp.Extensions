using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.UI.Navigation;

namespace J3space.Sample.Pages.Shared.Components.Profile
{
    public class ProfileViewComponent : AbpViewComponent
    {
        private readonly ILanguageProvider _languageProvider;
        private readonly IMenuManager _menuManager;

        public ProfileViewComponent(
            ILanguageProvider languageProvider,
            IMenuManager menuManager)
        {
            _languageProvider = languageProvider;
            _menuManager = menuManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = await _languageProvider.GetLanguagesAsync();
            var currentLanguage = language.FindByCulture(
                CultureInfo.CurrentCulture.Name,
                CultureInfo.CurrentUICulture.Name);
            var userMenu = await _menuManager.GetAsync(StandardMenus.User);
            var profile = new ProfileViewModel
            {
                CurrentLanguage = currentLanguage,
                OtherLanguages = language.Where(l => l != currentLanguage).ToList(),
                UserMenu = userMenu
            };

            return View("~/Pages/Shared/Components/Profile/Default.cshtml", profile);
        }
    }
}