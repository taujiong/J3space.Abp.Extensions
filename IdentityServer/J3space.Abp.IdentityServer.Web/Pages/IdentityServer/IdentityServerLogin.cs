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

        public override async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "Cancel")
            {
                var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
                if (context == null)
                {
                    return Redirect("~/");
                }

                await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                return Redirect(ReturnUrl);
            }

            if (action == "Register")
            {
                if (ModelState.GetFieldValidationState(nameof(RegisterInput)) !=
                    ModelValidationState.Valid)
                    return Page();

                await AccountAppService.RegisterAsync(RegisterInput);
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

            var loginResult = await AccountAppService.Login(LoginInput);

            if (loginResult.Result == LoginResultType.Success) return Redirect(ReturnUrl ?? "/");

            return Page();
        }
    }
}
