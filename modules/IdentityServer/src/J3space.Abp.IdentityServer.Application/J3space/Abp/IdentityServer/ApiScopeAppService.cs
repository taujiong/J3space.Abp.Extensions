using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.ApiScopes;
using J3space.Abp.IdentityServer.ApiScopes.Dto;
using J3space.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.ApiScopes;

namespace J3space.Abp.IdentityServer
{
    [Authorize(IdentityServerPermissions.ApiScope.Default)]
    public class ApiScopeAppService : IdentityServerAppServiceBase, IApiScopeAppService
    {
        private readonly IApiScopeRepository _apiScopeRepository;

        public ApiScopeAppService(IApiScopeRepository apiScopeRepository)
        {
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<PagedResultDto<ApiScopeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var list = await _apiScopeRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount, includeDetails: true);
            var totalCount = await _apiScopeRepository.GetCountAsync();

            return new PagedResultDto<ApiScopeDto>(
                totalCount,
                ObjectMapper.Map<List<ApiScope>, List<ApiScopeDto>>(list)
            );
        }

        public async Task<ApiScopeDto> GetAsync(Guid id)
        {
            var apiScope = await _apiScopeRepository.GetAsync(id);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        [Authorize(IdentityServerPermissions.ApiScope.Create)]
        public async Task<ApiScopeDto> CreateAsync(ApiScopeCreateUpdateDto input)
        {
            var existed = await _apiScopeRepository.CheckNameExistAsync(input.Name);
            if (existed)
                throw new UserFriendlyException(L["EntityExisted", nameof(ApiScope),
                    nameof(ApiScope.Name),
                    input.Name]);

            var apiScope = new ApiScope(GuidGenerator.Create(), input.Name);
            apiScope = ObjectMapper.Map(input, apiScope);
            input.UserClaims.ForEach(x => apiScope.AddUserClaim(x));
            input.Properties.ForEach(p => apiScope.AddProperty(p.Key, p.Value));

            apiScope = await _apiScopeRepository.InsertAsync(apiScope, true);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        [Authorize(IdentityServerPermissions.ApiScope.Update)]
        public async Task<ApiScopeDto> UpdateAsync(Guid id, ApiScopeCreateUpdateDto input)
        {
            var apiScope = await _apiScopeRepository.GetAsync(id);

            var existed = await _apiScopeRepository.CheckNameExistAsync(input.Name, id);
            if (existed)
                throw new UserFriendlyException(L["EntityExisted", nameof(ApiScope),
                    nameof(ApiScope.Name),
                    input.Name]);

            apiScope = ObjectMapper.Map(input, apiScope);
            apiScope.RemoveAllUserClaims();
            input.UserClaims
                .ForEach(x => apiScope.AddUserClaim(x));

            apiScope.RemoveAllProperties();
            input.Properties.ForEach(p => apiScope.AddProperty(p.Key, p.Value));

            apiScope = await _apiScopeRepository.UpdateAsync(apiScope);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        [Authorize(IdentityServerPermissions.ApiScope.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var client = await _apiScopeRepository.FindAsync(id);
            if (client == null) throw new EntityNotFoundException(typeof(ApiScope), id);

            await _apiScopeRepository.DeleteAsync(id);
        }
    }
}