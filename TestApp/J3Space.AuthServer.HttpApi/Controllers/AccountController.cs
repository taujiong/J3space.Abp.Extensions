using System;
using System.Threading.Tasks;
using J3space.AuthServer.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace J3space.AuthServer.Controllers
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area("account")]
    [Route("api/account")]
    public class AccountController : AuthServerController
    {
        private readonly Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> _signInManager;
        private readonly IdentityUserManager _userManager;

        public AccountController(
            Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<JsonResult> Login(UserLoginDto login)
        {
            ValidateUserLoginDto(login);
            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var signInResult = await _signInManager.PasswordSignInAsync(
                login.UserNameOrEmailAddress,
                login.Password,
                login.RememberMe,
                true);

            return GetLoginResult(signInResult);
        }

        [HttpGet]
        [Route("logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpPost]
        [Route("check-password")]
        public virtual async Task<JsonResult> CheckPassword(UserLoginDto login)
        {
            ValidateUserLoginDto(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var identityUser = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);

            if (identityUser == null)
            {
                return new JsonResult(LoginResultType.InvalidUserNameOrPassword);
            }

            return GetLoginResult(
                await _signInManager.CheckPasswordSignInAsync(
                    identityUser,
                    login.Password,
                    true)
            );
        }

        private JsonResult GetLoginResult(SignInResult signInResult)
        {
            if (signInResult.Succeeded)
            {
                return new JsonResult(new
                {
                    Result = LoginResultType.Success,
                    Description = L[LoginResultType.Success.ToString()].Value
                });
            }

            if (signInResult.RequiresTwoFactor)
            {
                return new JsonResult(new
                {
                    Result = LoginResultType.RequiresTwoFactor,
                    Description = L[LoginResultType.RequiresTwoFactor.ToString()].Value
                });
            }

            if (signInResult.IsLockedOut)
            {
                return new JsonResult(new
                {
                    Result = LoginResultType.LockedOut,
                    Description = L[LoginResultType.LockedOut.ToString()].Value
                });
            }

            if (signInResult.IsNotAllowed)
            {
                return new JsonResult(new
                {
                    Result = LoginResultType.NotAllowed,
                    Description = L[LoginResultType.NotAllowed.ToString()].Value
                });
            }

            return new JsonResult(new
            {
                Result = LoginResultType.InvalidUserNameOrPassword,
                Description = L[LoginResultType.InvalidUserNameOrPassword.ToString()].Value
            });
        }

        private void ValidateUserLoginDto(UserLoginDto login)
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

        private async Task ReplaceEmailToUsernameOfInputIfNeeds(UserLoginDto login)
        {
            if (!ValidationHelper.IsValidEmailAddress(login.UserNameOrEmailAddress))
            {
                return;
            }

            var userByUsername = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await _userManager.FindByEmailAsync(login.UserNameOrEmailAddress);
            if (userByEmail == null)
            {
                return;
            }

            login.UserNameOrEmailAddress = userByEmail.UserName;
        }
    }

    public enum LoginResultType : byte
    {
        Success = 0,

        InvalidUserNameOrPassword = 1,

        NotAllowed = 2,

        LockedOut = 3,

        RequiresTwoFactor = 4
    }
}
