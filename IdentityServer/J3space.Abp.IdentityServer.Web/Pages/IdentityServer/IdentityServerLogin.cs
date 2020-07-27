using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using J3space.Abp.Account;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace J3space.Abp.IdentityServer.Web.Pages.IdentityServer
{
    [ExposeServices(typeof(Login))]
    public class IdentityServerLogin : Login
    {
        private readonly IIdentityServerInteractionService _interaction;

        public IdentityServerLogin(
            IAccountAppService accountAppService,
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityServerInteractionService interaction,
            IdentityUserManager userManager, Microsoft.AspNetCore.Identity.SignInManager<IdentityUser> signInManager
        ) : base(accountAppService, schemeProvider, userManager, signInManager)
        {
            _interaction = interaction;
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginDto();

            await SetAvailableExternalLoginProviders();

            var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context != null)
            {
                LoginInput.UserNameOrEmailAddress = context.LoginHint;

                var tenant = context.Parameters[TenantResolverConsts.DefaultTenantKey];
                if (!string.IsNullOrEmpty(tenant))
                {
                    CurrentTenant.Change(Guid.Parse(tenant));
                    Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
                }
            }

            return Page();
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            // TODO: 取消的逻辑

            if (!ModelState.IsValid)
                return Page();

            AccountPageResult = await AccountAppService.Login(LoginInput);

            if (AccountPageResult.Succeed) return RedirectSafely(ReturnUrl, ReturnUrlHash);

            return Page();
        }
    }
}
