using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace J3space.Abp.Account
{
    public class AccountAppService : AccountAppServiceBase, IAccountAppService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityUserManager _userManager;

        public AccountAppService(
            IdentityUserManager userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress,
                CurrentTenant.Id);

            (await _userManager.CreateAsync(user, input.Password)).CheckErrors();

            await _userManager.AddDefaultRolesAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task<AccountResult> Login(LoginDto login)
        {
            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var signInResult = await _signInManager.PasswordSignInAsync(
                login.UserNameOrEmailAddress,
                login.Password,
                login.RememberMe,
                true
            );

            return GetAccountResult(signInResult);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AccountResult> CheckPassword(LoginDto login)
        {
            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            var identityUser = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);

            if (identityUser == null)
                return new AccountResult
                {
                    Succeed = false,
                    Message = L["Failed"]
                };

            var signInResult = await _signInManager.CheckPasswordSignInAsync(identityUser, login.Password, true);

            return GetAccountResult(signInResult);
        }

        private AccountResult GetAccountResult(SignInResult signInResult)
        {
            if (!signInResult.Succeeded)
                return new AccountResult
                {
                    Succeed = false,
                    Message = L[signInResult.ToString()]
                };

            return new AccountResult
            {
                Succeed = true,
                Message = L["SuccessLogin"]
            };
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(LoginDto login)
        {
            if (!ValidationHelper.IsValidEmailAddress(login.UserNameOrEmailAddress)) return;

            var userByUsername = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);
            if (userByUsername != null) return;

            var userByEmail = await _userManager.FindByEmailAsync(login.UserNameOrEmailAddress);
            if (userByEmail == null) return;

            login.UserNameOrEmailAddress = userByEmail.UserName;
        }

        protected virtual void ValidateLoginInfo(LoginDto login)
        {
            if (login == null) throw new ArgumentException(nameof(login));

            if (login.UserNameOrEmailAddress.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(login.UserNameOrEmailAddress));

            if (login.Password.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(login.Password));
        }
    }
}