using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace J3space.Abp.IdentityServer.IdentityResources
{
    public interface IIdentityResourceAppService : IApplicationService
    {
        public Task<PagedResultDto<IdentityResourceDto>> GetListAsync(
            PagedAndSortedResultRequestDto input);

        public Task<ListResultDto<IdentityResourceDto>> GetAllListAsync();
        public Task<IdentityResourceDto> GetAsync(Guid id);
        public Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateUpdateDto input);

        public Task<IdentityResourceDto>
            UpdateAsync(Guid id, IdentityResourceCreateUpdateDto input);

        public Task<JsonResult> DeleteAsync(Guid id);
    }
}