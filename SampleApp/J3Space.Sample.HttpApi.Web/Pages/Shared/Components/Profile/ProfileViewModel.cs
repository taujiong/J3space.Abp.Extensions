using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.UI.Navigation;

namespace J3space.Sample.Pages.Shared.Components.Profile
{
    public class ProfileViewModel
    {
        public LanguageInfo CurrentLanguage { get; set; }

        public List<LanguageInfo> OtherLanguages { get; set; }
        public ApplicationMenu UserMenu { get; set; }
    }
}