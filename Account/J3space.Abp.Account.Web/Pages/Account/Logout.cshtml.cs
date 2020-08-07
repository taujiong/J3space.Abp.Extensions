using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Logout : AccountPageModel
    {
        public Logout(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [BindProperty] protected string ReturnUrl { get; set; }
        [BindProperty] protected string ReturnUrlHash { get; set; }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            await AccountAppService.Logout();
            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }
    }
}