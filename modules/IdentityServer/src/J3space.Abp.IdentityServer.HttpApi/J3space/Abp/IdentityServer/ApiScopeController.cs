using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.ApiScopes;
using J3space.Abp.IdentityServer.ApiScopes.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace J3space.Abp.IdentityServer
{
    [RemoteService(Name = IdentityServerRemoteServiceConstants.RemoteServiceName)]
    [Area("identityServer")]
    [Route("api/ids/api-scopes")]
    public class ApiScopeController : IdentityServerControllerBase, IApiScopeAppService
    {
        private readonly IApiScopeAppService _apiScopeAppService;

        public ApiScopeController(IApiScopeAppService apiScopeAppService)
        {
            _apiScopeAppService = apiScopeAppService;
        }

        [HttpGet]
        public Task<PagedResultDto<ApiScopeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _apiScopeAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public Task<ApiScopeDto> GetAsync(Guid id)
        {
            return _apiScopeAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<ApiScopeDto> CreateAsync(ApiScopeCreateUpdateDto input)
        {
            return _apiScopeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public Task<ApiScopeDto> UpdateAsync(Guid id, ApiScopeCreateUpdateDto input)
        {
            return _apiScopeAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public Task DeleteAsync(Guid id)
        {
            return _apiScopeAppService.DeleteAsync(id);
        }
    }
}