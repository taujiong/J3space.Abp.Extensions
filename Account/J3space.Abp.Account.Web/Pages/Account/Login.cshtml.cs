using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace J3space.Abp.Account.Pages.Account
{
    public class Login : AccountPageModel
    {
        public Login(Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager,
            IdentityUserManager userManager
        )
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty] public LoginDto LoginInput { get; set; }

        [BindProperty] public RegisterDto RegisterInput { get; set; }

        private IdentityUserManager UserManager { get; }
        private Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> SignInManager { get; }

        public virtual IActionResult OnGetAsync()
        {
            LoginInput = new LoginDto();
            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await ReplaceEmailToUsernameOfInputIfNeeds();

            var result = await SignInManager.PasswordSignInAsync(
                LoginInput.UserNameOrEmailAddress,
                LoginInput.Password,
                LoginInput.RememberMe,
                true
            );

            if (result.IsLockedOut) return Page();

            if (result.IsNotAllowed) return Page();

            if (!result.Succeeded) return Page();

            //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
            var user = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
                       await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);

            Debug.Assert(user != null, nameof(user) + " != null");

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds()
        {
            if (!ValidationHelper.IsValidEmailAddress(LoginInput.UserNameOrEmailAddress)) return;

            var userByUsername =
                await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress);
            if (userByUsername != null) return;

            var userByEmail = await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);
            if (userByEmail == null) return;

            LoginInput.UserNameOrEmailAddress = userByEmail.UserName;
        }
    }
}
