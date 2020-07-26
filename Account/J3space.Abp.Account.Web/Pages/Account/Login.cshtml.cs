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

        // TODO: 依赖这里在页面上显示信息，重构一下实现所有异常的页面显示，而不仅仅是登录错误
        public AbpLoginResult LoginResult { get; set; } =
            new AbpLoginResult(LoginResultType.Success);

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

            LoginResult = await AccountAppService.Login(LoginInput);

            if (LoginResult.Result == LoginResultType.Success) return RedirectSafely(ReturnUrl, ReturnUrlHash);

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
                // TODO: 此处应在页面显示报错信息
                return RedirectToPage("./Login");

            var result = await SignInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                false,
                true
            );

            if (result.Succeeded) return RedirectSafely(returnUrl, returnUrlHash);

            /*
             * TODO: 不成功的话不一定是没有注册
             * result.IsLockedOut;
             * result.IsNotAllowed;
             * result.RequiresTwoFactor;
             */
            const string defaultExternalLoginUserPassword = "1q2w3E*"; // TODO: 默认密码放配置文件
            var userName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);
            var emailAddress = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress);
            user.AddLogin(loginInfo);
            (await UserManager.CreateAsync(user, defaultExternalLoginUserPassword)).CheckErrors();
            (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

            var loginResult = await SignInManager.PasswordSignInAsync(user.UserName,
                defaultExternalLoginUserPassword,
                false,
                false);

            if (loginResult.Succeeded) return RedirectSafely(returnUrl, returnUrlHash);

            // TODO: 错误处理
            return Page();
        }
    }
}