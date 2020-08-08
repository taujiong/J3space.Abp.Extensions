using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using J3space.Abp.Account.Web.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace J3space.Abp.IdentityServer.Web.Pages.Consent
{
    public class ConsentModel : AccountPageModel
    {
        private readonly IClientStore _clientStore;

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IResourceStore _resourceStore;

        public ConsentModel(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty] public ConsentInputModel ConsentInput { get; set; }

        public ClientInfoModel ClientInfo { get; set; }

        public virtual async Task<IActionResult> OnGet()
        {
            var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (request == null) throw new ApplicationException($"No consent request matching request: {ReturnUrl}");

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            if (client == null) throw new ApplicationException($"Invalid client id: {request.ClientId}");

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            if (resources == null || !resources.IdentityResources.Any() && !resources.ApiResources.Any())
                throw new ApplicationException(
                    $"No scopes matching: {request.ScopesRequested.Aggregate((x, y) => x + ", " + y)}");

            ClientInfo = new ClientInfoModel(client);
            ConsentInput = new ConsentInputModel
            {
                UserName = HttpContext.User.Identity.Name,
                RememberConsent = true,
                IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, true)).ToList(),
                ApiScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => CreateScopeViewModel(x, true))
                    .ToList()
            };

            if (resources.OfflineAccess) ConsentInput.ApiScopes.Add(GetOfflineAccessScope(true));

            return Page();
        }

        public virtual async Task<IActionResult> OnPost(string userDecision)
        {
            ValidateModel();

            ConsentInput.UserDecision = userDecision;

            var result = await ProcessConsentAsync();

            if (result.IsRedirect) return Redirect(result.RedirectUri);

            if (result.HasValidationError)
                //ModelState.AddModelError("", result.ValidationError);
                throw new ApplicationException("Error: " + result.ValidationError);

            throw new ApplicationException("Unknown Error!");
        }

        protected virtual async Task<ProcessConsentResult> ProcessConsentAsync()
        {
            var result = new ProcessConsentResult();

            ConsentResponse grantedConsent;

            if (ConsentInput.UserDecision == "no")
            {
                grantedConsent = ConsentResponse.Denied;
            }
            else
            {
                if (!ConsentInput.IdentityScopes.IsNullOrEmpty() || !ConsentInput.ApiScopes.IsNullOrEmpty())
                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = ConsentInput.RememberConsent,
                        ScopesConsented = ConsentInput.GetAllowedScopeNames()
                    };
                else
                    throw new UserFriendlyException("You must pick at least one permission");
            }

            if (grantedConsent != null)
            {
                var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
                if (request == null) return result;

                await _interaction.GrantConsentAsync(request, grantedConsent);

                result.RedirectUri = GetSafeRedirectUri(ReturnUrl, ReturnUrlHash);
            }

            return result;
        }

        protected virtual ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        protected virtual ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        protected virtual ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access",
                Description = "Access to your applications and resources, even when you are offline",
                Emphasize = true,
                Checked = check
            };
        }
    }
}