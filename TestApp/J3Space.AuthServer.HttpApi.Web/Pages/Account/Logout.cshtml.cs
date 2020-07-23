using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Identity;

namespace J3space.AuthServer.Pages.Account
{
    public class Logout : PageModel
    {
        private readonly Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private string LogoutId { get; set; }
        private string ReturnUrl { get; set; } = "/";

        public Logout(Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();

            LogoutId = HttpContext.Request.Query["logoutId"].ToString();
            if (!string.IsNullOrEmpty(LogoutId))
            {
                var logoutContext = await _interactionService.GetLogoutContextAsync(LogoutId);
                ReturnUrl = logoutContext.PostLogoutRedirectUri;
            }

            return Redirect(ReturnUrl);
        }
    }
}