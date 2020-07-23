using System;
using System.Threading.Tasks;
using J3space.Abp.Account.Settings;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace J3space.Abp.Account
{
    public class AccountAppService : AccountAppServiceBase, IAccountAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ISettingProvider _settingProvider;

        public AccountAppService(
            IdentityUserManager userManager,
            ISettingProvider settingProvider,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _settingProvider = settingProvider;
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            await CheckSelfRegistrationAsync();

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

            (await _userManager.CreateAsync(user, input.Password)).CheckErrors();

            await _userManager.SetEmailAsync(user,input.EmailAddress);
            await _userManager.AddDefaultRolesAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task<AbpLoginResult> Login(UserLoginInfo login)
        {
            await CheckLocalLoginAsync();

            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            return GetAbpLoginResult(await _signInManager.PasswordSignInAsync(
                login.UserNameOrEmailAddress,
                login.Password,
                login.RememberMe,
                true
            ));
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AbpLoginResult> CheckPassword(UserLoginInfo login)
        {
            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var identityUser = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);

            if (identityUser == null)
            {
                return new AbpLoginResult(LoginResultType.InvalidUserNameOrPassword);
            }

            return GetAbpLoginResult(await _signInManager.CheckPasswordSignInAsync(identityUser, login.Password, true));
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await _settingProvider.IsTrueAsync(AbpAccountSettingNames.IsSelfRegistrationEnabled))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }

                protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(UserLoginInfo login)
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

        private static AbpLoginResult GetAbpLoginResult(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return new AbpLoginResult(LoginResultType.LockedOut);
            }

            if (result.RequiresTwoFactor)
            {
                return new AbpLoginResult(LoginResultType.RequiresTwoFactor);
            }

            if (result.IsNotAllowed)
            {
                return new AbpLoginResult(LoginResultType.NotAllowed);
            }

            if (!result.Succeeded)
            {
                return new AbpLoginResult(LoginResultType.InvalidUserNameOrPassword);
            }

            return new AbpLoginResult(LoginResultType.Success);
        }

        protected virtual void ValidateLoginInfo(UserLoginInfo login)
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
            if (!await _settingProvider.IsTrueAsync(AbpAccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
            }
        }

    }
}
