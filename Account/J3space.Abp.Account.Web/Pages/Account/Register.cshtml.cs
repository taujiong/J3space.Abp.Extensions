using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Register : AccountPageModel
    {
        public Register(
            IAccountAppService accountAppService,
            IAuthenticationSchemeProvider schemeProvider
        ) : base(accountAppService, schemeProvider)
        {
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty] public RegisterDto RegisterInput { get; set; }

        public async Task<IActionResult> OnGet()
        {
            RegisterInput = new RegisterDto();

            await SetAvailableExternalLoginProviders();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await AccountAppService.RegisterAsync(RegisterInput);
            var loginInput = new LoginDto
            {
                Password = RegisterInput.Password,
                RememberMe = false,
                UserNameOrEmailAddress = RegisterInput.UserName
            };

            var loginResult = await AccountAppService.Login(loginInput);

            if (loginResult.Result == LoginResultType.Success) return RedirectSafely(ReturnUrl, ReturnUrlHash);

            // TODO: 错误处理
            return Page();
        }
    }
}