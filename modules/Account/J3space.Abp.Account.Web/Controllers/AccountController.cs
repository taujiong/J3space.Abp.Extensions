using System.Threading.Tasks;
using J3space.Abp.Account.Web.Models;
using Microsoft.AspNetCore.Http;
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
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace J3space.Abp.Account.Web.Controllers
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Controller]
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
        public virtual async Task<IActionResult> Login(LoginInputModel login)
        {
            await CheckLocalLoginAsync();

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
        public virtual async Task<IActionResult> CheckPassword(LoginInputModel login)
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var identityUser = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);

            if (identityUser == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    result = L["LoginFailed"],
                    detail = L["InvalidUserNameOrPassword"]
                });
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

        private IActionResult GetAbpLoginResult(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    result = L["LoginFailed"].Value,
                    detail = L["UserLockedOutMessage"].Value
                });
            }

            if (result.IsNotAllowed)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    result = L["LoginFailed"].Value,
                    detail = L["LoginIsNotAllowed"].Value
                });
            }

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    result = L["LoginFailed"].Value,
                    detail = L["InvalidUserNameOrPassword"].Value
                });
            }

            return Ok();
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