using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace J3space.Abp.IdentityServer.ApiResources
{
    public interface IApiResourceAppService : IApplicationService
    {
        public Task<PagedResultDto<ApiResourceDto>> GetListAsync(
            PagedAndSortedResultRequestDto input);

        public Task<ListResultDto<ApiResourceDto>> GetAllListAsync();
        public Task<ApiResourceDto> GetAsync(Guid id);
        public Task<ApiResourceDto> CreateAsync(ApiResourceCreateUpdateDto input);
        public Task<ApiResourceDto> UpdateAsync(Guid id, ApiResourceCreateUpdateDto input);
        public Task<JsonResult> DeleteAsync(Guid id);
    }
}