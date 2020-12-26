using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using J3space.Abp.Account.Web.Models;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace J3space.Abp.IdentityServer.Web.Pages.Account
{
    [ExposeServices(typeof(LoginModel))]
    public class IdentityServerSupportedLoginModel : LoginModel
    {
        public IdentityServerSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore)
            : base(schemeProvider)
        {
            Interaction = interaction;
            ClientStore = clientStore;
        }

        private IIdentityServerInteractionService Interaction { get; }
        private IClientStore ClientStore { get; }

        public override async Task<IActionResult> OnGetAsync()
        {
            Input = new LoginInputModel();

            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context != null)
            {
                Input.UserNameOrEmailAddress = context.LoginHint;

                //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
                var tenant = context.Parameters[TenantResolverConsts.DefaultTenantKey];
                if (!string.IsNullOrEmpty(tenant))
                {
                    CurrentTenant.Change(Guid.Parse(tenant));
                    Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
                }
            }

            if (context?.IdP != null)
            {
                Input.UserNameOrEmailAddress = context.LoginHint;
                ExternalProviderHelper.VisibleExternalProviders = new[]
                    {new ExternalProviderModel {AuthenticationScheme = context.IdP}};
                return Page();
            }

            await ExternalProviderHelper.GetVisibleExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            if (context?.Client?.ClientId != null)
            {
                var client = await ClientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    EnableLocalLogin = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        ExternalProviderHelper.VisibleExternalProviders =
                            ExternalProviderHelper.VisibleExternalProviders
                                .Where(provider =>
                                    client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme))
                                .ToList();
                    }
                }
            }

            return Page();
        }

        public override IActionResult OnGetCancel()
        {
            var context = Interaction.GetAuthorizationContextAsync(ReturnUrl).Result;
            if (context == null)
            {
                return Redirect("~/");
            }

            Interaction.GrantConsentAsync(context, new ConsentResponse
            {
                Error = AuthorizationError.LoginRequired
            }).Wait();

            return Redirect(ReturnUrl);
        }
    }
}