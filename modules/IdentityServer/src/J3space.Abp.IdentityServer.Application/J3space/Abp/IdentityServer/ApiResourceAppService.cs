using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.ApiResources;
using J3space.Abp.IdentityServer.ApiResources.Dto;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.ApiResources;

namespace J3space.Abp.IdentityServer
{
    [Authorize(IdentityServerPermissions.ApiResource.Default)]
    public class ApiResourceAppService : IdentityServerAppServiceBase, IApiResourceAppService
    {
        private readonly IApiResourceRepository _resourceRepository;

        public ApiResourceAppService(IApiResourceRepository resourceRepository
        )
        {
            _resourceRepository = resourceRepository;
        }

        public virtual async Task<PagedResultDto<ApiResourceDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var list = await _resourceRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount);
            var totalCount = await _resourceRepository.GetCountAsync();

            return new PagedResultDto<ApiResourceDto>(
                totalCount,
                ObjectMapper.Map<List<ApiResource>, List<ApiResourceDto>>(list)
            );
        }

        public virtual async Task<ApiResourceDto> GetAsync(Guid id)
        {
            var apiResource = await _resourceRepository.GetAsync(id);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(IdentityServerPermissions.ApiResource.Create)]
        public virtual async Task<ApiResourceDto> CreateAsync(ApiResourceCreateUpdateDto input)
        {
            var existed = await _resourceRepository.CheckNameExistAsync(input.Name);
            if (existed)
            {
                throw new UserFriendlyException(L["EntityExisted", nameof(ApiResource),
                    nameof(ApiResource.Name),
                    input.Name]);
            }

            var apiResource = new ApiResource(GuidGenerator.Create(), input.Name);
            apiResource = ObjectMapper.Map(input, apiResource);
            input.UserClaims.ForEach(x => apiResource.AddUserClaim(x));
            input.Scopes.ForEach(x => apiResource.AddScope(x));

            apiResource = await _resourceRepository.InsertAsync(apiResource, true);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(IdentityServerPermissions.ApiResource.Update)]
        public virtual async Task<ApiResourceDto> UpdateAsync(Guid id, ApiResourceCreateUpdateDto input)
        {
            var apiResource = await _resourceRepository.GetAsync(id);

            var existed = await _resourceRepository.CheckNameExistAsync(input.Name, id);
            if (existed)
            {
                throw new UserFriendlyException(L["EntityExisted", nameof(ApiResource),
                    nameof(ApiResource.Name),
                    input.Name]);
            }

            apiResource = ObjectMapper.Map(input, apiResource);

            apiResource.UserClaims.Clear();
            input.UserClaims
                .ForEach(x => apiResource.AddUserClaim(x));
            apiResource.Scopes.Clear();
            input.Scopes
                .ForEach(x => apiResource.AddScope(x));

            apiResource = await _resourceRepository.UpdateAsync(apiResource);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(IdentityServerPermissions.ApiResource.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var client = await _resourceRepository.FindAsync(id);
            if (client == null) throw new EntityNotFoundException(typeof(ApiResource), id);

            await _resourceRepository.DeleteAsync(id);
        }
    }
}