using System;
using J3space.Abp.Account.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class AccountPageModel : PageModel
    {
        protected IAccountAppService AccountAppService;

        protected AccountPageModel()
        {
            LocalizationResourceType = typeof(AbpAccountResource);
            AccountPageResult = new AccountResult
            {
                Succeed = true
            };
        }

        public AccountResult AccountPageResult { get; set; }

        protected virtual RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
        {
            return Redirect(GetSafeRedirectUri(returnUrl, returnUrlHash));
        }

        protected virtual string GetSafeRedirectUri(string returnUrl, string returnUrlHash = null)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = "~/";
            if (!string.IsNullOrWhiteSpace(returnUrlHash)) returnUrl += returnUrlHash;
            return returnUrl;
        }

        protected virtual void ValidateModel()
        {
            ModelValidator?.Validate(ModelState);
        }

        public class ExternalProviderModel
        {
            public string DisplayName { get; set; }
            public string AuthenticationScheme { get; set; }
        }

        #region code from AbpPageModel

        #region ServiceProvider

        public IServiceProvider ServiceProvider { get; set; }
        private readonly object ServiceProviderLock = new object();

        private TService LazyGetRequiredService<TService>(ref TService reference)
        {
            return LazyGetRequiredService(typeof(TService), ref reference);
        }

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
                lock (ServiceProviderLock)
                {
                    if (reference == null) reference = (TRef) ServiceProvider.GetRequiredService(serviceType);
                }

            return reference;
        }

        #endregion

        #region Localizer

        private IStringLocalizer _localizer;
        private IStringLocalizerFactory _stringLocalizerFactory;
        private IStringLocalizerFactory StringLocalizerFactory => LazyGetRequiredService(ref _stringLocalizerFactory);
        protected Type LocalizationResourceType { get; set; }

        protected IStringLocalizer L
        {
            get
            {
                if (_localizer == null) _localizer = CreateLocalizer();

                return _localizer;
            }
        }

        protected virtual IStringLocalizer CreateLocalizer()
        {
            if (LocalizationResourceType != null) return StringLocalizerFactory.Create(LocalizationResourceType);

            var localizer = StringLocalizerFactory.CreateDefaultOrNull();
            if (localizer == null)
                throw new AbpException(
                    $"Set {nameof(LocalizationResourceType)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");

            return localizer;
        }

        #endregion

        #region Others

        private IModelStateValidator ModelValidator => LazyGetRequiredService(ref _modelValidator);
        private IModelStateValidator _modelValidator;

        protected ICurrentTenant CurrentTenant => LazyGetRequiredService(ref _currentTenant);
        private ICurrentTenant _currentTenant;

        protected IGuidGenerator GuidGenerator => LazyGetRequiredService(ref _guidGenerator);
        private IGuidGenerator _guidGenerator;

        #endregion

        #endregion
    }
}