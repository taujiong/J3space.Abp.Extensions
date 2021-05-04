using System.Security.Claims;
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
            await SignInManager.SignOutAsync();

            var logoutId = Request.Query["logoutId"].ToString();

            if (!string.IsNullOrEmpty(logoutId))
            {
                var logoutContext = await Interaction.GetLogoutContextAsync(logoutId);

                await SaveSecurityLogAsync(logoutContext?.ClientId);

                HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
                var vm = new LoggedOutModel
                {
                    PostLogoutRedirectUri = logoutContext?.PostLogoutRedirectUri,
                    ClientName = logoutContext?.ClientName,
                    SignOutIframeUrl = logoutContext?.SignOutIFrameUrl
                };

                Logger.LogInformation("Redirecting to LoggedOut Page...");

                return RedirectToPage("./LoggedOut", vm);
            }

            await SaveSecurityLogAsync();

            if (ReturnUrl == null)
            {
                Logger.LogInformation(
                    "{ClassName} couldn't find postLogoutUri... Redirecting to the index page",
                    nameof(IdentityServerSupportedLogoutModel));
            }

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }

        protected virtual async Task SaveSecurityLogAsync(string clientId = null)
        {
            if (CurrentUser.IsAuthenticated)
            {
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.Logout,
                    ClientId = clientId
                });
            }
        }
    }
}