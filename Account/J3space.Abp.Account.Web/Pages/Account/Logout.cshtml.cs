using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class Logout : PageModel
    {
        protected readonly IAccountAppService AccountAppService;

        public Logout(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [BindProperty] protected string ReturnUrl { get; set; } = "/";

        public virtual async Task<IActionResult> OnGetAsync()
        {
            await AccountAppService.Logout();
            return Redirect(ReturnUrl);
        }
    }
}
