﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.ApiResources;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;

namespace J3space.Abp.IdentityServer
{
    [Authorize(IdentityServerPermissions.ApiResource.Default)]
    public class ApiResourceAppService : IdentityServerAppServiceBase, IApiResourceAppService
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IGuidGenerator _guidGenerator;

        public ApiResourceAppService(
            IApiResourceRepository apiResourceRepository,
            IGuidGenerator guidGenerator
        )
        {
            _apiResourceRepository = apiResourceRepository;
            _guidGenerator = guidGenerator;
        }

        public virtual async Task<PagedResultDto<ApiResourceDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var list = await _apiResourceRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount);
            var totalCount = await _apiResourceRepository.GetCountAsync();

            return new PagedResultDto<ApiResourceDto>(
                totalCount,
                ObjectMapper.Map<List<ApiResource>, List<ApiResourceDto>>(list)
            );
        }

        public virtual async Task<ApiResourceDto> GetAsync(Guid id)
        {
            var apiResource = await _apiResourceRepository.FindAsync(id);
            if (apiResource == null) throw new EntityNotFoundException(typeof(ApiResource), id);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(IdentityServerPermissions.ApiResource.Create)]
        public virtual async Task<ApiResourceDto> CreateAsync(ApiResourceCreateUpdateDto input)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(input.Name);
            if (apiResource == null)
            {
                apiResource = new ApiResource(_guidGenerator.Create(), input.Name);
                await _apiResourceRepository.InsertAsync(apiResource, true);
            }

            return await UpdateAsync(apiResource.Id, input);
        }

        [Authorize(IdentityServerPermissions.ApiResource.Update)]
        public virtual async Task<ApiResourceDto> UpdateAsync(Guid id, ApiResourceCreateUpdateDto input)
        {
            var apiResource = await _apiResourceRepository.FindAsync(id);
            if (apiResource == null) throw new EntityNotFoundException(typeof(ApiResource), id);

            apiResource.Description = input.Description.IsNullOrEmpty()
                ? input.Name
                : input.Description;
            apiResource.DisplayName = input.Description.IsNullOrEmpty()
                ? input.Name
                : input.DisplayName;
            apiResource.Enabled = input.Enabled;

            if (input.Scopes != null)
            {
                var oldList = ObjectMapper.Map<List<ApiScope>, List<string>>(apiResource.Scopes);

                var toBeRemoved = oldList.Except(input.Scopes);
                foreach (var scope in toBeRemoved) apiResource.RemoveScope(scope);

                var toBeAdd = input.Scopes.Except(oldList);
                foreach (var scope in toBeAdd) apiResource.AddScope(scope);
            }

            if (input.UserClaims != null)
            {
                var oldList =
                    ObjectMapper.Map<List<ApiResourceClaim>, List<string>>(apiResource.UserClaims);

                var toBeRemoved = oldList.Except(input.UserClaims);
                foreach (var claim in toBeRemoved) apiResource.RemoveClaim(claim);

                var toBeAdd = input.UserClaims.Except(oldList);
                foreach (var claim in toBeAdd) apiResource.AddUserClaim(claim);
            }

            apiResource = await _apiResourceRepository.UpdateAsync(apiResource);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(IdentityServerPermissions.ApiResource.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var client = await _apiResourceRepository.FindAsync(id);
            if (client == null) throw new EntityNotFoundException(typeof(ApiResource), id);

            await _apiResourceRepository.DeleteAsync(id);
        }
    }
}