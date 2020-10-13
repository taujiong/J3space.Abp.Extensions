using System.Threading.Tasks;
using IdentityServer4.Services;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace J3space.Abp.IdentityServer.Web.Pages.Account
{
    [ExposeServices(typeof(LogoutModel))]
    public class IdentityServerSupportedLogoutModel : LogoutModel
    {
        private IIdentityServerInteractionService Interaction { get; }

        public IdentityServerSupportedLogoutModel(IIdentityServerInteractionService interaction)
        {
            Interaction = interaction;
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout
            });

            await SignInManager.SignOutAsync();

            var logoutId = Request.Query["logoutId"].ToString();

            if (!string.IsNullOrEmpty(logoutId))
            {
                var logoutContext = await Interaction.GetLogoutContextAsync(logoutId);
                ReturnUrl = logoutContext?.PostLogoutRedirectUri;
            }

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }

            Logger.LogInformation(
                $"{nameof(IdentityServerSupportedLogoutModel)} couldn't find postLogoutUri... Redirecting to:/Account/Login..");
            return RedirectToPage("/Account/Login");
        }
    }
}