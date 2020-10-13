using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.IdentityResources;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.IdentityResources;

namespace J3space.Abp.IdentityServer
{
    [Authorize(IdentityServerPermissions.IdentityResource.Default)]
    public class IdentityResourceAppService : IdentityServerAppServiceBase, IIdentityResourceAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityResourceRepository _resourceRepository;

        public IdentityResourceAppService(
            IIdentityResourceRepository resourceRepository,
            IGuidGenerator guidGenerator)
        {
            _resourceRepository = resourceRepository;
            _guidGenerator = guidGenerator;
        }

        public virtual async Task<PagedResultDto<IdentityResourceDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var list = await _resourceRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount);
            var totalCount = await _resourceRepository.GetCountAsync();

            return new PagedResultDto<IdentityResourceDto>(
                totalCount,
                ObjectMapper.Map<List<IdentityResource>, List<IdentityResourceDto>>(
                    list)
            );
        }

        public virtual async Task<IdentityResourceDto> GetAsync(Guid id)
        {
            var identityResource = await _resourceRepository.FindAsync(id);
            if (identityResource == null) throw new EntityNotFoundException(typeof(IdentityResource), id);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(IdentityServerPermissions.IdentityResource.Create)]
        public virtual async Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateUpdateDto input)
        {
            var identityResource = await _resourceRepository.FindByNameAsync(input.Name);
            if (identityResource == null)
            {
                identityResource = new IdentityResource(_guidGenerator.Create(), input.Name);
                await _resourceRepository.InsertAsync(identityResource, true);
            }

            return await UpdateAsync(identityResource.Id, input);
        }

        [Authorize(IdentityServerPermissions.IdentityResource.Update)]
        public virtual async Task<IdentityResourceDto> UpdateAsync(Guid id,
            IdentityResourceCreateUpdateDto input)
        {
            var identityResource = await _resourceRepository.FindAsync(id);
            if (identityResource == null) throw new EntityNotFoundException(typeof(IdentityResource), id);

            identityResource.Description = input.Description.IsNullOrEmpty()
                ? input.Name
                : input.Description;
            identityResource.DisplayName = input.Description.IsNullOrEmpty()
                ? input.Name
                : input.DisplayName;
            identityResource.Emphasize = input.Emphasize;
            identityResource.Enabled = input.Enabled;
            identityResource.Required = input.Required;
            identityResource.ShowInDiscoveryDocument = input.ShowInDiscoveryDocument;

            if (input.UserClaims != null)
            {
                var oldList =
                    ObjectMapper.Map<List<IdentityClaim>, List<string>>(identityResource
                        .UserClaims);

                var toBeRemoved = oldList.Except(input.UserClaims);
                foreach (var claim in toBeRemoved) identityResource.RemoveUserClaim(claim);

                var toBeAdd = input.UserClaims.Except(oldList);
                foreach (var claim in toBeAdd) identityResource.AddUserClaim(claim);
            }

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