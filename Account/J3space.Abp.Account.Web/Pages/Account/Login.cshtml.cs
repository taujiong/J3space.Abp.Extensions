using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Login : AccountPageModel
    {
        protected readonly SignInManager<IdentityUser> SignInManager;
        protected readonly IdentityUserManager UserManager;

        public Login(
            IAccountAppService accountAppService,
            IAuthenticationSchemeProvider schemeProvider,
            IdentityUserManager userManager,
            SignInManager<IdentityUser> signInManager
        ) : base(accountAppService, schemeProvider)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]

        public string ReturnUrlHash { get; set; }

        [BindProperty] public LoginDto LoginInput { get; set; }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginDto();

            await SetAvailableExternalLoginProviders();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            AccountPageResult = await AccountAppService.Login(LoginInput);

            if (AccountPageResult.Succeed) return RedirectSafely(ReturnUrl, ReturnUrlHash);

            return Page();
        }

        public virtual async Task<IActionResult> OnGetExternalLogin(string provider, string returnUrl,
            string returnUrlHash)
        {
            var redirectUrl = Url.Page("./Login", "ExternalLoginCallback", new {returnUrl, returnUrlHash});
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["scheme"] = provider;

            return await Task.FromResult(Challenge(properties, provider));
        }

        public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl, string returnUrlHash)
        {
            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                AccountPageResult.Succeed = false;
                AccountPageResult.Message = L["NullExternalLoginInfo"];
                return Page();
            }

            var result = await SignInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                false,
                true
            );

            if (result.Succeeded) return RedirectSafely(returnUrl, returnUrlHash);

            if (result.IsLockedOut || result.IsNotAllowed || result.RequiresTwoFactor)
            {
                AccountPageResult = new AccountResult
                {
                    Succeed = false,
                    Message = L[result.ToString()]
                };

                return Page();
            }

            // 到这里基本上就能确定是没有注册了
            const string defaultExternalLoginUserPassword = "1q2w3E*"; // TODO: 默认密码放配置文件
            var userName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);
            var emailAddress = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress);
            user.AddLogin(loginInfo);
            (await UserManager.CreateAsync(user, defaultExternalLoginUserPassword))
                .CheckErrors(); // TODO: 考虑第三方获取的用户名被占用的情形
            (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

            var loginResult = await SignInManager.PasswordSignInAsync(user.UserName,
                defaultExternalLoginUserPassword,
                false,
                false);

            if (loginResult.Succeeded) return RedirectSafely(returnUrl, returnUrlHash);

            AccountPageResult.Succeed = false;
            AccountPageResult.Message = L[loginResult.ToString()];
            return Page();
        }
    }
}
