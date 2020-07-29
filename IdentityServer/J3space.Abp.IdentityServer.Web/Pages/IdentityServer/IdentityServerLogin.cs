using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using J3space.Abp.Account;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace J3space.Abp.IdentityServer.Web.Pages.IdentityServer
{
    [ExposeServices(typeof(Login))]
    public class IdentityServerLogin : Login
    {
        private readonly IIdentityServerInteractionService _interaction;

        public IdentityServerLogin(
            IAccountAppService accountAppService,
            IIdentityServerInteractionService interaction,
            IdentityUserManager userManager, Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager
        ) : base(accountAppService, userManager, signInManager)
        {
            _interaction = interaction;
        }

        public override async Task<IActionResult> OnGetCancelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context == null) return Redirect("~/");

            await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

            return Redirect(returnUrl);
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            AccountPageResult = await AccountAppService.Login(LoginInput);

            if (AccountPageResult.Succeed) return RedirectSafely(ReturnUrl, ReturnUrlHash);

            return Page();
        }
    }
}
