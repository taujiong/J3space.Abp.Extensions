using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;

namespace J3space.Abp.IdentityServer.TestBase
{
    public class AbpIdentityServerTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        private readonly AbpIdentityServerTestData _testData;

        public AbpIdentityServerTestDataBuilder(
            IGuidGenerator guidGenerator,
            IApiResourceRepository apiResourceRepository,
            IClientRepository clientRepository,
            IIdentityResourceRepository identityResourceRepository,
            IIdentityClaimTypeRepository identityClaimTypeRepository,
            AbpIdentityServerTestData testData
        )
        {
            _testData = testData;
            _guidGenerator = guidGenerator;
            _apiResourceRepository = apiResourceRepository;
            _clientRepository = clientRepository;
            _identityResourceRepository = identityResourceRepository;
            _identityClaimTypeRepository = identityClaimTypeRepository;
        }

        public async Task BuildAsync()
        {
            await AddIdentityResources();
            await AddApiResources();
            await AddClients();
            await AddClaimTypes();
        }

        private async Task AddIdentityResources()
        {
            var identityResource = new IdentityResource(_testData.IdentityResource1Id, "NewIdentityResource1")
            {
                Description = nameof(Client.Description),
                DisplayName = nameof(IdentityResource.DisplayName)
            };

            identityResource.AddUserClaim(nameof(ApiResourceClaim.Type));

            await _identityResourceRepository.InsertAsync(identityResource);
            await _identityResourceRepository.InsertAsync(new IdentityResource(_guidGenerator.Create(),
                "NewIdentityResource2"));
            await _identityResourceRepository.InsertAsync(new IdentityResource(_guidGenerator.Create(),
                "NewIdentityResource3"));
        }

        private async Task AddApiResources()
        {
            var apiResource = new ApiResource(_testData.ApiResource1Id, "NewApiResource1");
            apiResource.Description = nameof(apiResource.Description);
            apiResource.DisplayName = nameof(apiResource.DisplayName);

            apiResource.AddScope(nameof(ApiScope.Name));
            apiResource.AddUserClaim(nameof(ApiResourceClaim.Type));
            apiResource.AddSecret(nameof(ApiSecret.Value));

            await _apiResourceRepository.InsertAsync(apiResource);
            await _apiResourceRepository.InsertAsync(new ApiResource(_guidGenerator.Create(), "NewApiResource2"));
            await _apiResourceRepository.InsertAsync(new ApiResource(_guidGenerator.Create(), "NewApiResource3"));
        }

        private async Task AddClients()
        {
            var client = new Client(_testData.Client1Id, "ClientId1")
            {
                Description = nameof(Client.Description),
                ClientName = nameof(Client.ClientName),
                ClientUri = nameof(Client.ClientUri),
                LogoUri = nameof(Client.LogoUri),
                ProtocolType = nameof(Client.ProtocolType),
                FrontChannelLogoutUri = nameof(Client.FrontChannelLogoutUri)
            };

            client.AddCorsOrigin("https://client1-origin.com");
            client.AddClaim(nameof(ClientClaim.Value), nameof(ClientClaim.Type));
            client.AddGrantType(nameof(ClientGrantType.GrantType));
            client.AddIdentityProviderRestriction(nameof(ClientIdPRestriction.Provider));
            client.AddPostLogoutRedirectUri(nameof(ClientPostLogoutRedirectUri.PostLogoutRedirectUri));
            client.AddProperty(nameof(ClientProperty.Key), nameof(ClientProperty.Value));
            client.AddRedirectUri(nameof(ClientRedirectUri.RedirectUri));
            client.AddScope(nameof(ClientScope.Scope));
            client.AddSecret(nameof(ClientSecret.Value));

            await _clientRepository.InsertAsync(client);

            await _clientRepository.InsertAsync(new Client(_guidGenerator.Create(), "ClientId2"));
            await _clientRepository.InsertAsync(new Client(_guidGenerator.Create(), "ClientId3"));
        }

        private async Task AddClaimTypes()
        {
            var ageClaim = new IdentityClaimType(Guid.NewGuid(), "Age", false, false, null, null, null,
                IdentityClaimValueType.Int);
            await _identityClaimTypeRepository.InsertAsync(ageClaim);
        }
    }
}