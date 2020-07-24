using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace J3space.Abp.Account.Pages.Account
{
    public class Login : PageModel
    {
        private readonly IAccountAppService _accountAppService;

        public Login(
            IAccountAppService accountAppService
        )
        {
            _accountAppService = accountAppService;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty] public LoginDto LoginInput { get; set; }

        [BindProperty] public RegisterDto RegisterInput { get; set; }

        public virtual IActionResult OnGetAsync()
        {
            LoginInput = new LoginDto();
            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "Register")
            {
                if (ModelState.GetFieldValidationState(nameof(RegisterInput)) !=
                    ModelValidationState.Valid)
                    return Page();

                await _accountAppService.RegisterAsync(RegisterInput);
                LoginInput = new LoginDto
                {
                    Password = RegisterInput.Password,
                    RememberMe = false,
                    UserNameOrEmailAddress = RegisterInput.UserName
                };
            }
            else if (action == "Login")
            {
                if (ModelState.GetFieldValidationState(nameof(LoginInput)) !=
                    ModelValidationState.Valid)
                    return Page();
            }

            var loginResult = await _accountAppService.Login(LoginInput);

            if (loginResult.Result == LoginResultType.Success) return Redirect(ReturnUrl ?? "/");

            return Page();
        }
    }
}
