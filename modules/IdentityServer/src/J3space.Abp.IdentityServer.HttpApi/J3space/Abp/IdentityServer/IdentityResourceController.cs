using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.IdentityResources;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace J3space.Abp.IdentityServer
{
    [RemoteService(Name = IdentityServerRemoteServiceConstants.RemoteServiceName)]
    [Area("identityServer")]
    [Route("api/ids/identity-resources")]
    public class IdentityResourceController : IdentityServerControllerBase, IIdentityResourceAppService
    {
        private readonly IIdentityResourceAppService _identityResourceAppService;

        public IdentityResourceController(IIdentityResourceAppService identityResourceAppService)
        {
            _identityResourceAppService = identityResourceAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<IdentityResourceDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            return _identityResourceAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<IdentityResourceDto> GetAsync(Guid id)
        {
            return _identityResourceAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateUpdateDto input)
        {
            return _identityResourceAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<IdentityResourceDto> UpdateAsync(Guid id, IdentityResourceCreateUpdateDto input)
        {
            return _identityResourceAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _identityResourceAppService.DeleteAsync(id);
        }
    }
}