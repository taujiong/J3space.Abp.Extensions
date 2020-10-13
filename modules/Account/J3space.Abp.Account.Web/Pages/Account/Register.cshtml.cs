using System.Linq;
using System.Threading.Tasks;
using J3space.Abp.Account.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.Settings;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class RegisterModel : AccountPageModel
    {
        public RegisterModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
            ExternalProviderHelper = new ExternalProviderHelper(schemeProvider, accountOptions.Value);
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty(SupportsGet = true)] public RegisterInputModel Input { get; set; }

        [BindProperty(SupportsGet = true)] public bool IsExternalLogin { get; set; }
        public ExternalProviderHelper ExternalProviderHelper { get; }

        public virtual async Task<IActionResult> OnGetAsync(string userName, string email)
        {
            await CheckSelfRegistrationAsync();
            await ExternalProviderHelper.GetVisibleExternalProviders();

            Input = new RegisterInputModel
            {
                UserName = userName,
                EmailAddress = email
            };

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await CheckSelfRegistrationAsync();

                if (IsExternalLogin)
                {
                    var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        Logger.LogWarning("External login info is not available");
                        return RedirectToPage("./Login");
                    }

                    await RegisterExternalUserAsync(externalLoginInfo, Input.EmailAddress);
                }
                else
                {
                    await RegisterLocalUserAsync();
                }

                return Redirect(ReturnUrl ?? "~/");
            }
            catch (BusinessException e)
            {
                Alerts.Danger(e.Message);
                return Page();
            }
        }

        protected virtual async Task RegisterLocalUserAsync()
        {
            ValidateModel();

            var userDto = await AccountAppService.RegisterAsync(
                new RegisterDto
                {
                    AppName = "MVC",
                    EmailAddress = Input.EmailAddress,
                    Password = Input.Password,
                    UserName = Input.UserName
                }
            );

            var user = await UserManager.GetByIdAsync(userDto.Id);
            await SignInManager.SignInAsync(user, true);
        }

        protected virtual async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string emailAddress)
        {
            var userDto = await AccountAppService.RegisterAsync(
                new RegisterDto
                {
                    AppName = "MVC",
                    EmailAddress = Input.EmailAddress,
                    Password = Input.Password,
                    UserName = Input.UserName
                }
            );

            var user = await UserManager.GetByIdAsync(userDto.Id);

            var userLoginAlreadyExists = user.Logins.Any(x =>
                x.TenantId == user.TenantId &&
                x.LoginProvider == externalLoginInfo.LoginProvider &&
                x.ProviderKey == externalLoginInfo.ProviderKey);

            if (!userLoginAlreadyExists)
            {
                (await UserManager.AddLoginAsync(user, new UserLoginInfo(
                    externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey,
                    externalLoginInfo.ProviderDisplayName
                ))).CheckErrors();
            }

            await SignInManager.SignInAsync(user, true);
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
                !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }
    }
}