using System.Threading.Tasks;
using IdentityServer4.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityServer.Clients;

namespace J3space.Abp.IdentityServer.Web
{
    public class IdentityServerCorsPolicyService : ICorsPolicyService, ISingletonDependency
    {
        private readonly IClientRepository _clientRepository;

        public IdentityServerCorsPolicyService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var allowedCors = await _clientRepository.GetAllDistinctAllowedCorsOriginsAsync();
            return allowedCors.Contains(origin);
        }
    }
}