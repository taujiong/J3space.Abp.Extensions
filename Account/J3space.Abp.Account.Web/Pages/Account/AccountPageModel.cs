using J3space.Abp.Account.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class AccountPageModel : AbpPageModel
    {
        protected readonly IAccountAppService AccountAppService;

        protected AccountPageModel(
            IAccountAppService accountAppService
        )
        {
            AccountAppService = accountAppService;
            LocalizationResourceType = typeof(AbpAccountResource);
            ObjectMapperContext = typeof(AbpAccountWebModule);
            AccountPageResult = new AccountResult
            {
                Succeed = true
            };
        }

        public AccountResult AccountPageResult { get; set; }

        protected RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = "~/";

            if (!string.IsNullOrWhiteSpace(returnUrlHash)) returnUrl += returnUrlHash;

            return Redirect(returnUrl);
        }

        public class ExternalProviderModel
        {
            public string DisplayName { get; set; }
            public string AuthenticationScheme { get; set; }
        }
    }
}
