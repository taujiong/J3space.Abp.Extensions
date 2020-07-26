using System.Threading.Tasks;
using IdentityServer4.Services;
using J3space.Abp.Account;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace J3space.Abp.IdentityServer.Web.Pages.IdentityServer
{
    [ExposeServices(typeof(Login))]
    public class IdentityServerLogin : Login
    {
        private readonly IIdentityServerInteractionService _interaction;

        public IdentityServerLogin(
            IAccountAppService accountAppService,
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityServerInteractionService interaction,
            Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signManager,
            IdentityUserManager userManager,
            IGuidGenerator guidGenerator
        ) : base(accountAppService, schemeProvider, signManager, userManager, guidGenerator)
        {
            _interaction = interaction;
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginDto();

            var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context != null) LoginInput.UserNameOrEmailAddress = context.LoginHint;

            return Page();
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            LoginResult = await AccountAppService.Login(LoginInput);

            if (LoginResult.Result == LoginResultType.Success) return Redirect(ReturnUrl ?? "/");

            return Page();
        }
    }
}
