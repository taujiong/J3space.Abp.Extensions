using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.IdentityResources;
using J3space.Abp.IdentityServer.IdentityResources.Dto;
using J3space.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.IdentityResources;

namespace J3space.Abp.IdentityServer
{
    [Authorize(IdentityServerPermissions.IdentityResource.Default)]
    public class IdentityResourceAppService : IdentityServerAppServiceBase, IIdentityResourceAppService
    {
        private readonly IIdentityResourceRepository _resourceRepository;

        public IdentityResourceAppService(IIdentityResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public virtual async Task<PagedResultDto<IdentityResourceDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var list = await _resourceRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount, includeDetails: true);
            var totalCount = await _resourceRepository.GetCountAsync();

            return new PagedResultDto<IdentityResourceDto>(
                totalCount,
                ObjectMapper.Map<List<IdentityResource>, List<IdentityResourceDto>>(list)
            );
        }

        public virtual async Task<IdentityResourceDto> GetAsync(Guid id)
        {
            var identityResource = await _resourceRepository.GetAsync(id);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(IdentityServerPermissions.IdentityResource.Create)]
        public virtual async Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateUpdateDto input)
        {
            var existed = await _resourceRepository.CheckNameExistAsync(input.Name);
            if (existed)
                throw new UserFriendlyException(L["EntityExisted", nameof(IdentityResource),
                    nameof(IdentityResource.Name),
                    input.Name]);

            var identityResource = new IdentityResource(GuidGenerator.Create(), input.Name);
            identityResource = ObjectMapper.Map(input, identityResource);
            input.UserClaims.ForEach(x => identityResource.AddUserClaim(x));
            input.Properties.ForEach(p => identityResource.AddProperty(p.Key, p.Value));

            identityResource = await _resourceRepository.InsertAsync(identityResource, true);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(IdentityServerPermissions.IdentityResource.Update)]
        public virtual async Task<IdentityResourceDto> UpdateAsync(Guid id,
            IdentityResourceCreateUpdateDto input)
        {
            var identityResource = await _resourceRepository.GetAsync(id);

            var existed = await _resourceRepository.CheckNameExistAsync(input.Name, id);
            if (existed)
                throw new UserFriendlyException(L["EntityExisted", nameof(IdentityResource),
                    nameof(IdentityResource.Name),
                    input.Name]);

            identityResource = ObjectMapper.Map(input, identityResource);
            identityResource.RemoveAllUserClaims();
            input.UserClaims
                .ForEach(x => identityResource.AddUserClaim(x));

            identityResource.RemoveAllProperties();
            input.Properties.ForEach(p => identityResource.AddProperty(p.Key, p.Value));

            identityResource = await _resourceRepository.UpdateAsync(identityResource);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(IdentityServerPermissions.IdentityResource.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var client = await _resourceRepository.FindAsync(id);
            if (client == null) throw new EntityNotFoundException(typeof(IdentityResource), id);

            await _resourceRepository.DeleteAsync(id);
        }
    }
}