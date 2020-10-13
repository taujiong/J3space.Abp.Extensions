using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using J3space.Abp.Account.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Settings;
using Volo.Abp.Validation;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class LoginModel : AccountPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty] public LoginInputModel Input { get; set; }

        public bool EnableLocalLogin { get; set; }
        public ExternalProviderHelper ExternalProviderHelper { get; }

        public LoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions)
        {
            ExternalProviderHelper = new ExternalProviderHelper(schemeProvider, accountOptions.Value);
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            Input = new LoginInputModel();
            await ExternalProviderHelper.GetVisibleExternalProviders();
            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            return Page();
        }

        public virtual IActionResult OnGetCancel()
        {
            return Redirect("~/");
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            await CheckLocalLoginAsync();

            ValidateModel();

            await ReplaceEmailToUsernameOfInputIfNeeds();

            var result = await SignInManager.PasswordSignInAsync(
                Input.UserNameOrEmailAddress,
                Input.Password,
                Input.RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = Input.UserNameOrEmailAddress
            });

            if (result.RequiresTwoFactor)
            {
                return await TwoFactorLoginResultAsync();
            }

            if (result.IsLockedOut)
            {
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
            }

            if (result.IsNotAllowed)
            {
                Alerts.Warning(L["LoginIsNotAllowed"]);
                return Page();
            }

            if (!result.Succeeded)
            {
                Alerts.Danger(L["InvalidUserNameOrPassword"]);
                return Page();
            }

            //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
            var user = await UserManager.FindByNameAsync(Input.UserNameOrEmailAddress) ??
                       await UserManager.FindByEmailAsync(Input.UserNameOrEmailAddress);

            Debug.Assert(user != null, nameof(user) + " != null");

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }

        /// <summary>
        /// Override this method to add 2FA for your application.
        /// </summary>
        protected virtual Task<IActionResult> TwoFactorLoginResultAsync()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IActionResult> OnGetExternalLoginAsync(string provider)
        {
            var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback",
                values: new {ReturnUrl, ReturnUrlHash});
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["scheme"] = provider;

            return await Task.FromResult(Challenge(properties, provider));
        }

        public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "",
            string returnUrlHash = "", string remoteError = null)
        {
            if (remoteError != null)
            {
                Logger.LogWarning($"External login callback error: {remoteError}");
                return RedirectToPage("./Login");
            }

            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                Logger.LogWarning("External login info is not available");
                return RedirectToPage("./Login");
            }

            var result = await SignInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                false,
                true
            );

            if (!result.Succeeded)
            {
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                    Action = "Login" + result
                });
            }

            if (result.IsLockedOut)
            {
                throw new UserFriendlyException("Cannot proceed because user is locked out!");
            }

            if (result.Succeeded)
            {
                return RedirectSafely(returnUrl, returnUrlHash);
            }

            return RedirectToPage("./Register", new
            {
                IsExternalLogin = true,
                UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name),
                Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                ReturnUrl = returnUrl,
                ReturnUrlHash = returnUrlHash
            });
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds()
        {
            if (!ValidationHelper.IsValidEmailAddress(Input.UserNameOrEmailAddress))
            {
                return;
            }

            var userByUsername = await UserManager.FindByNameAsync(Input.UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(Input.UserNameOrEmailAddress);
            if (userByEmail == null)
            {
                return;
            }

            Input.UserNameOrEmailAddress = userByEmail.UserName;
        }

        protected virtual async Task CheckLocalLoginAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
            }
        }
    }
}