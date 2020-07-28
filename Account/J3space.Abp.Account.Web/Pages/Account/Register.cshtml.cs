using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Register : AccountPageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityUserManager _userManager;

        public Register(
            IAccountAppService accountAppService,
            IAuthenticationSchemeProvider schemeProvider,
            SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager
        ) : base(accountAppService, schemeProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty] public RegisterDto RegisterInput { get; set; }

        public async Task<IActionResult> OnGet(string userName, string emailAddress)
        {
            RegisterInput = new RegisterDto
            {
                UserName = userName,
                EmailAddress = emailAddress
            };

            await SetAvailableExternalLoginProviders();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var user = new IdentityUser(GuidGenerator.Create(), RegisterInput.UserName, RegisterInput.EmailAddress,
                CurrentTenant.Id);

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo != null) user.AddLogin(externalLoginInfo);

            var userCreateResult = await _userManager.CreateAsync(user, RegisterInput.Password);
            if (userCreateResult.Errors.Any())
            {
                AccountPageResult.Succeed = false;
                foreach (var error in userCreateResult.Errors)
                    // TODO: 错误信息国际化
                    AccountPageResult.Message += error.Description;

                await SetAvailableExternalLoginProviders();

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
