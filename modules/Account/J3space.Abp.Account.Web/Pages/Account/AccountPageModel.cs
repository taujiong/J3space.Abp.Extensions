using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Validation;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public abstract class AccountPageModel : AbpPageModel
    {
        protected AccountPageModel()
        {
            MyAlerts = new AlertList();
            LocalizationResourceType = typeof(AccountResource);
        }

        public IAccountAppService AccountAppService { get; set; }
        public Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> SignInManager { get; set; }
        public IdentityUserManager UserManager { get; set; }
        public IdentitySecurityLogManager IdentitySecurityLogManager { get; set; }
        public AlertList MyAlerts { get; }

        protected virtual RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
        {
            return Redirect(GetRedirectUrl(returnUrl, returnUrlHash));
        }

        protected virtual string GetRedirectUrl(string returnUrl, string returnUrlHash = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (!returnUrlHash.IsNullOrWhiteSpace())
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return returnUrl;
        }

        protected virtual string NormalizeReturnUrl(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                return GetAppHomeUrl();
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return GetAppHomeUrl();
        }

        protected virtual void CheckCurrentTenant(Guid? tenantId)
        {
            if (CurrentTenant.Id != tenantId)
            {
                throw new ApplicationException(
                    $"Current tenant is different than given tenant. CurrentTenant.Id: {CurrentTenant.Id}, given tenantId: {tenantId}");
            }
        }

        protected virtual string GetAppHomeUrl()
        {
            return "~/"; //TODO: ???
        }

        protected string GetMessageFromException(Exception ex)
        {
            var message = ex switch
            {
                UserFriendlyException e => e.Message,
                AbpIdentityResultException e => e.LocalizeMessage(new LocalizationContext(ServiceProvider))
                    .Replace(",", "\n"),
                AbpValidationException e => e.ValidationErrors.Select(error => error.ErrorMessage).JoinAsString("\n"),
                BusinessException e => L[e.Code, ex.Data["Email"]],
                _ => ex.Message
            };

            return message;
        }
    }
}