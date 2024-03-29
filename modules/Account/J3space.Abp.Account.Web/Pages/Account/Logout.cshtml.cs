using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class LogoutModel : AccountPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout
            });

            await SignInManager.SignOutAsync();

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }

        public virtual Task<IActionResult> OnPostAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }
    }
}