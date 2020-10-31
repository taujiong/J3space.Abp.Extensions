using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace J3space.Auth.DbMigrator
{
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;

        public IdentityServerDataSeedContributor(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _configuration = configuration;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();

            var idsConfiguration = _configuration.GetSection("IdentityServer");

            var apiResources = idsConfiguration.GetSection("ApiResources").GetChildren();
            foreach (var apiResource in apiResources)
            {
                var claims = apiResource.GetSection("Claims").Get<List<string>>();
                await CreateApiResourceAsync(apiResource.Key, claims);
            }

            var clients = idsConfiguration.GetSection("Clients").GetChildren();
            foreach (var client in clients)
            {
                var grantTypes = client.GetSection("GrantTypes").Get<List<string>>();
                var scopes = client.GetSection("Scopes").Get<List<string>>();

                await CreateClientAsync(
                    client.Key,
                    client["ClientUrl"],
                    scopes,
                    grantTypes,
                    client["ClientSecret"].Sha256()
                );
            }
        }

        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
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
                    autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task<Client> CreateClientAsync(
            string name,
            string clientUrl,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret = null)
        {
            var client = await _clientRepository.FindByCliendIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client(
                        _guidGenerator.Create(),
                        name
                    )
                    {
                        ClientName = name,
                        Description = name
                    },
                    autoSave: true
                );
            }

            if (!string.IsNullOrEmpty(clientUrl))
            {
                if (client.FindRedirectUri(clientUrl) == null)
                {
                    client.AddRedirectUri(clientUrl);
                }
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (!string.IsNullOrEmpty(secret))
            {
                if (client.FindSecret(secret) == null)
                {
                    client.AddSecret(secret);
                }
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}