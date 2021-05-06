using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using J3space.Abp.Account.Web.Pages.Account;
using J3space.Abp.IdentityServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace J3space.Abp.IdentityServer.Web.Pages.Account
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

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (request == null) throw new ApplicationException($"No consent request matching request: {ReturnUrl}");

            var client = await _clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);
            if (client == null) throw new ApplicationException($"Invalid client id: {request.Client.ClientId}");

            var resources =
                await _resourceStore.FindEnabledResourcesByScopeAsync(request.ValidatedResources.RawScopeValues);
            if (resources == null || !resources.IdentityResources.Any() && !resources.ApiResources.Any())
                throw new ApplicationException(
                    $"No scopes matching: {request.ValidatedResources.RawScopeValues.Aggregate((x, y) => x + ", " + y)}");

            ClientInfo = new ClientInfoModel(client);
            ConsentInput = new ConsentInputModel
            {
                UserName = HttpContext.User.Identity?.Name,
                RememberConsent = true,
                IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, true)).ToList()
            };

            var apiScopes = new List<ScopeViewModel>();
            foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
            {
                var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
                if (apiScope == null) continue;
                var scopeVm = CreateScopeViewModel(parsedScope, apiScope, true);
                apiScopes.Add(scopeVm);
            }

            if (resources.OfflineAccess) apiScopes.Add(GetOfflineAccessScope(true));

            ConsentInput.ApiScopes = apiScopes;

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync(string userDecision)
        {
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
                grantedConsent = new ConsentResponse
                {
                    Error = AuthorizationError.AccessDenied
                };
            else
                grantedConsent = new ConsentResponse
                {
                    RememberConsent = ConsentInput.RememberConsent,
                    ScopesValuesConsented = ConsentInput.GetAllowedScopeNames()
                };

            var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (request == null) return result;

            await _interaction.GrantConsentAsync(request, grantedConsent);

            result.RedirectUri = GetRedirectUrl(ReturnUrl, ReturnUrlHash);

            return result;
        }

        protected virtual ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new()
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        protected virtual ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope,
            bool check)
        {
            var displayName = apiScope.DisplayName ?? apiScope.Name;
            if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
                displayName += ":" + parsedScopeValue.ParsedParameter;

            return new ScopeViewModel
            {
                Name = parsedScopeValue.RawValue,
                DisplayName = displayName,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = check || apiScope.Required
            };
        }

        protected virtual ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new()
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access", //TODO: Localize
                Description = "Access to your applications and resources, even when you are offline",
                Emphasize = true,
                Checked = check
            };
        }
    }
}