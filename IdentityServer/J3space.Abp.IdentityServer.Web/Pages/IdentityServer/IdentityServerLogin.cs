using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using J3space.Abp.Account;
using J3space.Abp.Account.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace J3space.Abp.IdentityServer.Web.Pages.IdentityServer
{
    [ExposeServices(typeof(Login))]
    public class IdentityServerLogin : Login
    {
        private readonly IClientStore _clientStore;
        private readonly IEventService _identityServerEvents;
        private readonly IIdentityServerInteractionService _interaction;

        public IdentityServerLogin(
            IAccountAppService accountAppService,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService identityServerEvents
        ) : base(accountAppService)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _identityServerEvents = identityServerEvents;
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
            await AccountAppService.Logout();
            return Page();
        }
    }
}
