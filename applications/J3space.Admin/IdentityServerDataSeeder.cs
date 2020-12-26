using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace J3space.Admin
{
    public class IdentityServerDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public IdentityServerDataSeeder(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder,
            IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
            _configuration = configuration;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();
            await CreateApiResourcesAsync();
            await CreateClientsAsync();
        }

        private async Task CreateApiResourcesAsync()
        {
            var commonApiUserClaims = new[]
            {
                "email",
                "email_verified",
                "name",
                "phone_number",
                "phone_number_verified",
                "role"
            };

            await CreateApiResourceAsync("J3Admin", commonApiUserClaims);
            await CreateApiResourceAsync("J3Guard", commonApiUserClaims);
            await CreateApiResourceAsync("J3Blogging", commonApiUserClaims);
        }

        private async Task CreateApiResourceAsync(string name,
            IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    true
                );
            }

            foreach (var claim in claims)
                if (apiResource.FindClaim(claim) == null)
                    apiResource.AddUserClaim(claim);

            await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task CreateClientsAsync()
        {
            var idsDataSeedSection = _configuration.GetSection("SeedData:IdentityServer");

            var clients = idsDataSeedSection.GetSection("Clients").GetChildren();
            foreach (var client in clients)
            {
                var clientId = client.GetValue<string>("ClientId");
                if (!clientId.IsNullOrWhiteSpace())
                {
                    var clientRootUrl = client.GetValue<string>("RootUrl").EnsureEndsWith('/');
                    var clientSecret = client.GetValue("ClientSecret", "1q2w3e*").Sha256();
                    var clientScopes = client.GetValue<string>("Scopes").Split(' ');

                    await CreateClientAsync(
                        clientId,
                        clientScopes,
                        new[] {"authorization_code"},
                        clientSecret,
                        $"{clientRootUrl}swagger/oauth2-redirect.html"
                    );
                }
            }
        }

        private async Task CreateClientAsync(string name,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret,
            string redirectUri = null,
            string postLogoutRedirectUri = null,
            string frontChannelLogoutUri = null,
            IEnumerable<string> permissions = null)
        {
            var client = await _clientRepository.FindByClientIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client(
                        _guidGenerator.Create(),
                        name
                    )
                    {
                        ClientName = name,
                        ProtocolType = "oidc",
                        Description = name,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowOfflineAccess = true,
                        AbsoluteRefreshTokenLifetime = 31536000, //365 days
                        AccessTokenLifetime = 31536000, //365 days
                        AuthorizationCodeLifetime = 300,
                        IdentityTokenLifetime = 300,
                        RequireConsent = false,
                        FrontChannelLogoutUri = frontChannelLogoutUri
                    },
                    true
                );

                foreach (var scope in scopes)
                    if (client.FindScope(scope) == null)
                        client.AddScope(scope);

                foreach (var grantType in grantTypes)
                    if (client.FindGrantType(grantType) == null)
                        client.AddGrantType(grantType);

                if (client.FindSecret(secret) == null) client.AddSecret(secret);

                if (redirectUri != null)
                    if (client.FindRedirectUri(redirectUri) == null)
                        client.AddRedirectUri(redirectUri);

                if (postLogoutRedirectUri != null)
                    if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                        client.AddPostLogoutRedirectUri(postLogoutRedirectUri);

                if (permissions != null)
                    await _permissionDataSeeder.SeedAsync(
                        ClientPermissionValueProvider.ProviderName,
                        name,
                        permissions
                    );

                await _clientRepository.UpdateAsync(client);
            }
        }
    }
}