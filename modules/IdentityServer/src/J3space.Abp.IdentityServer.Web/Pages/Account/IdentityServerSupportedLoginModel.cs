using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using J3space.Abp.Account.Web;
using J3space.Abp.Account.Web.Models;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace J3space.Abp.IdentityServer.Web.Pages.Account
{
    [ExposeServices(typeof(LoginModel))]
    public class IdentityServerSupportedLoginModel : LoginModel
    {
        private IIdentityServerInteractionService Interaction { get; }
        private IClientStore ClientStore { get; }
        private AbpAccountOptions AccountOptions { get; }

        public IdentityServerSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore)
            : base(
                schemeProvider,
                accountOptions)
        {
            Interaction = interaction;
            ClientStore = clientStore;
            AccountOptions = accountOptions.Value;
        }

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

            if (context?.ClientId != null)
            {
                var client = await ClientStore.FindEnabledClientByIdAsync(context.ClientId);
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

            Interaction.GrantConsentAsync(context, ConsentResponse.Denied).Wait();

            return Redirect(ReturnUrl);
        }

        public override async Task<IActionResult> OnGetExternalLoginAsync(string provider)
        {
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync();
            }

            return await base.OnGetExternalLoginAsync(provider);
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync()
        {
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (!(result?.Principal is WindowsPrincipal windowsPrincipal))
            {
                return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
            }

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback",
                    values: new {ReturnUrl, ReturnUrlHash}),
                Items =
                {
                    {"scheme", AccountOptions.WindowsAuthenticationSchemeName},
                }
            };

            var identity = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, windowsPrincipal.Identity.Name));
            identity.AddClaim(new Claim(JwtClaimTypes.Name, windowsPrincipal.Identity.Name));

            //TODO: Consider to add Windows groups the the identity
            //if (_accountOptions.IncludeWindowsGroups)
            //{
            //    var windowsIdentity = windowsPrincipal.Identity as WindowsIdentity;
            //    if (windowsIdentity != null)
            //    {
            //        var groups = windowsIdentity.Groups?.Translate(typeof(NTAccount));
            //        var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
            //        identity.AddClaims(roles);
            //    }
            //}

            await HttpContext.SignInAsync(
                IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
                new ClaimsPrincipal(identity),
                props
            );

            return RedirectSafely(props.RedirectUri);
        }
    }
}