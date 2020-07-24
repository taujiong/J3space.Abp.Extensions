using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;

namespace J3space.Abp.Account.Pages.Account
{
    public class AccountPageModel : PageModel
    {
        protected virtual RedirectResult RedirectSafely(string returnUrl,
            string returnUrlHash = null)
        {
            return Redirect(GetRedirectUrl(returnUrl, returnUrlHash));
        }

        protected virtual void CheckIdentityErrors(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
                throw new UserFriendlyException("Operation failed: " + identityResult.Errors
                    .Select(e => $"[{e.Code}] {e.Description}")
                    .JoinAsString(", "));

            //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
        }

        protected virtual string GetRedirectUrl(string returnUrl, string returnUrlHash = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (!returnUrlHash.IsNullOrWhiteSpace()) returnUrl = returnUrl + returnUrlHash;

            return returnUrl;
        }

        protected virtual string NormalizeReturnUrl(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty()) return GetAppHomeUrl();

            if (Url.IsLocalUrl(returnUrl)) return returnUrl;

            return GetAppHomeUrl();
        }

        protected virtual string GetAppHomeUrl()
        {
            return "~/"; //TODO: ???
        }
    }
}