using System;
using System.Threading.Tasks;
using J3space.Abp.Account.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Settings;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace J3space.Abp.Account.Web.Controllers
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Controller]
    [ControllerName("Account")]
    [Route("api/account")]
    public class AccountController : AbpController
    {
        public AccountController(Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager,
            ISettingProvider settingProvider,
            IdentitySecurityLogManager identitySecurityLogManager)
        {
            LocalizationResource = typeof(AccountResource);

            SignInManager = signInManager;
            UserManager = userManager;
            SettingProvider = settingProvider;
            IdentitySecurityLogManager = identitySecurityLogManager;
        }

        protected Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> SignInManager { get; }
        protected IdentityUserManager UserManager { get; }
        protected ISettingProvider SettingProvider { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }

        [HttpPost]
        [Route("login")]
        public virtual async Task<LoginResult> Login(LoginInputModel login)
        {
            await CheckLocalLoginAsync();

            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);
            var signInResult = await SignInManager.PasswordSignInAsync(
                login.UserNameOrEmailAddress,
                login.Password,
                login.RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = signInResult.ToIdentitySecurityLogAction(),
                UserName = login.UserNameOrEmailAddress
            });

            return GetAbpLoginResult(signInResult);
        }

        [HttpGet]
        [Route("logout")]
        public virtual async Task Logout()
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout
            });

            await SignInManager.SignOutAsync();
        }

        [HttpPost]
        [Route("check-password")]
        public virtual async Task<LoginResult> CheckPassword(LoginInputModel login)
        {
            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var identityUser = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);

            if (identityUser == null)
            {
                return new LoginResult(LoginResultType.InvalidUserNameOrPassword);
            }

            return GetAbpLoginResult(await SignInManager.CheckPasswordSignInAsync(identityUser, login.Password, true));
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(LoginInputModel login)
        {
            if (!ValidationHelper.IsValidEmailAddress(login.UserNameOrEmailAddress))
            {
                return;
            }

            var userByUsername = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await UserManager.FindByEmailAsync(login.UserNameOrEmailAddress);
            if (userByEmail == null)
            {
                return;
            }

            login.UserNameOrEmailAddress = userByEmail.UserName;
        }

        private static LoginResult GetAbpLoginResult(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return new LoginResult(LoginResultType.LockedOut);
            }

            if (result.RequiresTwoFactor)
            {
                return new LoginResult(LoginResultType.RequiresTwoFactor);
            }

            if (result.IsNotAllowed)
            {
                return new LoginResult(LoginResultType.NotAllowed);
            }

            if (!result.Succeeded)
            {
                return new LoginResult(LoginResultType.InvalidUserNameOrPassword);
            }

            return new LoginResult(LoginResultType.Success);
        }

        protected virtual void ValidateLoginInfo(LoginInputModel login)
        {
            if (login == null)
            {
                throw new ArgumentException(nameof(login));
            }

            if (login.UserNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(login.UserNameOrEmailAddress));
            }

            if (login.Password.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(login.Password));
            }
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