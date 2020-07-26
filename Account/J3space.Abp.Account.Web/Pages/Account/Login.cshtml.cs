using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Login : PageModel
    {
        protected readonly IAccountAppService AccountAppService;
        protected readonly IGuidGenerator GuidGenerator;
        protected readonly IAuthenticationSchemeProvider SchemeProvider;
        protected readonly SignInManager<IdentityUser> SignInManager;
        protected readonly IdentityUserManager UserManager;

        public Login(
            IAccountAppService accountAppService,
            IAuthenticationSchemeProvider schemeProvider,
            SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager,
            IGuidGenerator guidGenerator
        )
        {
            AccountAppService = accountAppService;
            SchemeProvider = schemeProvider;
            SignInManager = signInManager;
            UserManager = userManager;
            GuidGenerator = guidGenerator;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]

        public string ReturnUrlHash { get; set; }

        [BindProperty] public LoginDto LoginInput { get; set; }

        public AbpLoginResult LoginResult { get; set; } =
            new AbpLoginResult(LoginResultType.Success);

        public IEnumerable<ExternalProviderModel> AvailableExternalProviders { get; set; }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginDto();

            var schemes = await SchemeProvider.GetAllSchemesAsync();
            AvailableExternalProviders = schemes
                .Where(x => !string.IsNullOrWhiteSpace(x.DisplayName))
                .Select(x => new ExternalProviderModel
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                })
                .ToList();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            LoginResult = await AccountAppService.Login(LoginInput);

            if (LoginResult.Result == LoginResultType.Success) return Redirect(ReturnUrl ?? "/");

            return Page();
        }

        public virtual async Task<IActionResult> OnGetExternalLogin(string provider)
        {
            if (string.IsNullOrEmpty(ReturnUrl)) ReturnUrl = "~/";

            if (!Url.IsLocalUrl(ReturnUrl))
                // TODO: 页面返回错误信息
                throw new UserFriendlyException("invalid return URL");

            var redirectUrl = Url.Page("./Login", "ExternalLoginCallback", new {ReturnUrl, ReturnUrlHash});
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["scheme"] = provider;

            return await Task.FromResult(Challenge(properties, provider));
        }

        public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync()
        {
            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return RedirectToPage("./Login");

            var result = await SignInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                false,
                true
            );

            if (result.Succeeded) return Redirect(ReturnUrl);

            // TODO: 不成功的话是不是就一定是没有注册呢
            const string defaultExternalLoginUserPassword = "1q2w3E*";
            var userName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);
            var emailAddress = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new IdentityUser(GuidGenerator.Create(), userName, emailAddress);
            user.AddLogin(loginInfo);
            (await UserManager.CreateAsync(user, defaultExternalLoginUserPassword)).CheckErrors(); // TODO: 默认密码放配置文件
            await UserManager.AddDefaultRolesAsync(user);
            var loginResult = await SignInManager.PasswordSignInAsync(user.UserName,
                defaultExternalLoginUserPassword,
                false,
                false);

            if (loginResult.Succeeded) return Redirect(ReturnUrl);

            return Page();
        }

        public class ExternalProviderModel
        {
            public string DisplayName { get; set; }
            public string AuthenticationScheme { get; set; }
        }
    }
}
