using System.Threading.Tasks;
using IdentityServer4.Services;
using J3space.Abp.Account;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace J3space.Abp.IdentityServer.Web.Pages.IdentityServer
{
    [ExposeServices(typeof(Logout))]
    public class IdentityServerLogout : Logout
    {
        private readonly IIdentityServerInteractionService _interaction;

        public IdentityServerLogout(
            IAccountAppService accountAppService,
            IIdentityServerInteractionService interaction
        ) : base(accountAppService)
        {
            _interaction = interaction;
        }

        private string LogoutId { get; set; }

        public override async Task<IActionResult> OnGetAsync()
        {
            await AccountAppService.Logout();

            LogoutId = HttpContext.Request.Query["logoutId"].ToString();
            if (!string.IsNullOrEmpty(LogoutId))
            {
                var logoutContext = await _interaction.GetLogoutContextAsync(LogoutId);
                ReturnUrl = logoutContext.PostLogoutRedirectUri;
                return Redirect(ReturnUrl);
            }

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }
    }
}