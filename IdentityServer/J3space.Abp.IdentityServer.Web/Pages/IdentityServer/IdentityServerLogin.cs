using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using J3space.Abp.Account;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.DependencyInjection;

namespace J3space.Abp.IdentityServer.Web.Pages.IdentityServer
{
    [ExposeServices(typeof(Login))]
    public class IdentityServerLogin : Login
    {
        private readonly IIdentityServerInteractionService _interaction;

        public IdentityServerLogin(
            IAccountAppService accountAppService,
            IIdentityServerInteractionService interaction
        ) : base(accountAppService)
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

            var loginResult = await AccountAppService.Login(LoginInput);

            if (loginResult.Result == LoginResultType.Success) return Redirect(ReturnUrl ?? "/");

            return Page();
        }
    }
}
