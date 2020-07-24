using System.Threading.Tasks;
using IdentityServer4.Services;
using J3space.Abp.Account;
using J3space.Abp.Account.Pages.Account;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace J3space.Abp.IdentityServer.Web.Pages.IdentityServer
{
    [ExposeServices(typeof(Login))]
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

        public override async Task<IActionResult> OnGetAsync()
        {
            await AccountAppService.Logout();
            return Redirect(ReturnUrl);
        }
    }
}
