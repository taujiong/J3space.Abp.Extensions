using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.ApiScopes.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace J3space.Abp.IdentityServer.ApiScopes
{
    public interface IApiScopeAppService : IApplicationService
    {
        public Task<PagedResultDto<ApiScopeDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        public Task<ApiScopeDto> GetAsync(Guid id);
        public Task<ApiScopeDto> CreateAsync(ApiScopeCreateUpdateDto input);
        public Task<ApiScopeDto> UpdateAsync(Guid id, ApiScopeCreateUpdateDto input);
        public Task DeleteAsync(Guid id);
    }
}