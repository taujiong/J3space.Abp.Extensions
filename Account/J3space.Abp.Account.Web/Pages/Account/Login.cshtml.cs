using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Login : AccountPageModel
    {
        protected readonly Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> SignInManager;

        public Login(
            IAccountAppService accountAppService,
            Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager
        )
        {
            AccountAppService = accountAppService;
            SignInManager = signInManager;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]

        public string ReturnUrlHash { get; set; }

        [BindProperty] public LoginDto LoginInput { get; set; }

        public virtual IActionResult OnGet(string userNameOrEmailAddress)
        {
            LoginInput = new LoginDto
            {
                UserNameOrEmailAddress = userNameOrEmailAddress
            };

            return Page();
        }

#pragma warning disable 1998
        public virtual async Task<IActionResult> OnGetCancelAsync(string returnUrl)
#pragma warning restore 1998
        {
            return Redirect("~/");
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

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
            var registerDto = new RegisterDto
            {
                UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name),
                EmailAddress = loginInfo.Principal.FindFirstValue(ClaimTypes.Email)
            };

            return RedirectToPage("./Register", new
            {
                registerDto.UserName,
                registerDto.EmailAddress,
                returnUrl,
                returnUrlHash
            });
        }
    }
}