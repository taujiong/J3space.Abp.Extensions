using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Register : PageModel
    {
        private readonly IAccountAppService _accountAppService;

        public Register(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty]public RegisterDto RegisterInput { get; set; }

        public IActionResult OnGet()
        {
            RegisterInput = new RegisterDto();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _accountAppService.RegisterAsync(RegisterInput);
            var loginInput = new LoginDto
            {
                Password = RegisterInput.Password,
                RememberMe = false,
                UserNameOrEmailAddress = RegisterInput.UserName
            };

            var loginResult = await _accountAppService.Login(loginInput);

            if (loginResult.Result == LoginResultType.Success) return Redirect(ReturnUrl ?? "/");

            return Page();
        }
    }
}
