using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Register : AccountPageModel
    {
        private readonly IStringLocalizer<IdentityResource> _localizer;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityUserManager _userManager;

        public Register(
            IAccountAppService accountAppService,
            SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager,
            IStringLocalizer<IdentityResource> localizer
        )
        {
            AccountAppService = accountAppService;
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty] public RegisterDto RegisterInput { get; set; }

        public virtual IActionResult OnGet(string userName, string emailAddress)
        {
            RegisterInput = new RegisterDto
            {
                UserName = userName,
                EmailAddress = emailAddress
            };

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var user = new IdentityUser(
                GuidGenerator.Create(),
                RegisterInput.UserName, RegisterInput.EmailAddress,
                CurrentTenant.Id
            );

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo != null) user.AddLogin(externalLoginInfo);

            var userCreateResult = await _userManager.CreateAsync(user, RegisterInput.Password);
            await _userManager.AddDefaultRolesAsync(user);
            if (userCreateResult.Errors.Any())
            {
                AccountPageResult.Succeed = false;
                foreach (var error in userCreateResult.Errors)
                    AccountPageResult.Message += $"{error.LocalizeErrorMessage(_localizer)}|";

                return Page();
            }

            var loginInput = new LoginDto
            {
                Password = RegisterInput.Password,
                RememberMe = false,
                UserNameOrEmailAddress = RegisterInput.UserName
            };

            AccountPageResult = await AccountAppService.Login(loginInput);

            if (!AccountPageResult.Succeed) return Page();

            if (externalLoginInfo?.LoginProvider != null)
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }
    }
}