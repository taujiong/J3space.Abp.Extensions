using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J3space.Abp.Account.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class AccountPageModel : AbpPageModel
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        protected readonly IAccountAppService AccountAppService;

        protected AccountPageModel(
            IAccountAppService accountAppService,
            IAuthenticationSchemeProvider schemeProvider
        )
        {
            AccountAppService = accountAppService;
            _schemeProvider = schemeProvider;
            LocalizationResourceType = typeof(AbpAccountResource);
            ObjectMapperContext = typeof(AbpAccountWebModule);
            AccountPageResult = new AccountResult
            {
                Succeed = true
            };
        }

        public IEnumerable<ExternalProviderModel> AvailableExternalProviders { get; set; }
        public AccountResult AccountPageResult { get; set; }

        protected RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = "~/";

            if (!string.IsNullOrWhiteSpace(returnUrlHash)) returnUrl += returnUrlHash;

            return Redirect(returnUrl);
        }

        protected async Task SetAvailableExternalLoginProviders()
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();
            AvailableExternalProviders = schemes
                .Where(x => !string.IsNullOrWhiteSpace(x.DisplayName))
                .Select(x => new ExternalProviderModel
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                })
                .ToList();
        }

        public class ExternalProviderModel
        {
            public string DisplayName { get; set; }
            public string AuthenticationScheme { get; set; }
        }
    }
}
