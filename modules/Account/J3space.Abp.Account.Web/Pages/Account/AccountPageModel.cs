using System;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public abstract class AccountPageModel : AbpPageModel
    {
        protected AccountPageModel()
        {
            MyAlerts = new AlertList();
            LocalizationResourceType = typeof(AccountResource);
        }

        protected IAccountAppService AccountAppService { get; set; }
        protected Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> SignInManager { get; set; }
        protected IdentityUserManager UserManager { get; set; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; set; }
        public AlertList MyAlerts { get; }
        private IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; set; }

        protected virtual void CheckCurrentTenant(Guid? tenantId)
        {
            if (CurrentTenant.Id != tenantId)
            {
                throw new ApplicationException(
                    $"Current tenant is different than given tenant. CurrentTenant.Id: {CurrentTenant.Id}, given tenantId: {tenantId}");
            }
        }

        protected virtual string GetLocalizeExceptionMessage(Exception exception)
        {
            if (exception is ILocalizeErrorMessage or IHasErrorCode)
            {
                return ExceptionToErrorInfoConverter.Convert(exception, false).Message;
            }

            return exception.Message;
        }
    }
}