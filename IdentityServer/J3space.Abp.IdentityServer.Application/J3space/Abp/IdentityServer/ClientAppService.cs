using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.PermissionManagement;

namespace J3space.Abp.IdentityServer
{
    [Authorize(IdentityServerPermissions.Client.Default)]
    public class ClientAppService : IdentityServerAppServiceBase, IClientAppService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionManager _permissionManager;

        public ClientAppService(
            IClientRepository clientRepository,
            IGuidGenerator guidGenerator,
            IPermissionManager permissionManager)
        {
            _clientRepository = clientRepository;
            _guidGenerator = guidGenerator;
            _permissionManager = permissionManager;
        }

        public async Task<PagedResultDto<ClientDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var list = await _clientRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount);
            var totalCount = await _clientRepository.GetCountAsync();

            return new PagedResultDto<ClientDto>(
                totalCount,
                ObjectMapper.Map<List<Client>, List<ClientDto>>(list)
            );
        }

        public async Task<ListResultDto<ClientDto>> GetAllListAsync()
        {
            var list = await _clientRepository.GetListAsync();

            return new ListResultDto<ClientDto>(
                ObjectMapper.Map<List<Client>, List<ClientDto>>(list)
            );
        }

        public async Task<ClientDto> GetAsync(Guid id)
        {
            var client = await _clientRepository.FindAsync(id);
            if (client == null)
            {
                throw new EntityNotFoundException(typeof(Client), id);
            }

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        [Authorize(IdentityServerPermissions.Client.Create)]
        public async Task<ClientDto> CreateAsync(ClientCreateUpdateDto input)
        {
            var client = await _clientRepository.FindByCliendIdAsync(input.ClientId);
            if (client == null)
            {
                client = new Client(_guidGenerator.Create(), input.ClientId)
                {
                    ClientName = input.ClientId,
                    ProtocolType = "oidc"
                };
                await _clientRepository.InsertAsync(client, true);
            }

            return await UpdateAsync(client.Id, input);
        }

        [Authorize(IdentityServerPermissions.Client.Update)]
        public async Task<ClientDto> UpdateAsync(Guid id, ClientCreateUpdateDto input)
        {
            var client = await _clientRepository.FindAsync(id);
            if (client == null)
            {
                throw new EntityNotFoundException(typeof(Client), id);
            }

            client.Description = input.Description.IsNullOrEmpty()
                ? input.ClientId
                : input.Description;
            client.RequireConsent = input.RequireConsent;

            #region 常用属性更新

            if (input.ClientSecrets != null)
            {
                var oldList =
                    ObjectMapper.Map<List<ClientSecret>, List<string>>(client.ClientSecrets);

                var toBeRemoved = oldList.Except(input.ClientSecrets);
                foreach (var secret in toBeRemoved)
                {
                    client.RemoveSecret(secret);
                }

                var toBeAdd = input.ClientSecrets.Except(oldList);
                foreach (var secret in toBeAdd)
                {
                    client.AddSecret(secret);
                }
            }

            if (input.AllowedScopes != null)
            {
                var oldList =
                    ObjectMapper.Map<List<ClientScope>, List<string>>(client.AllowedScopes);

                var toBeRemoved = oldList.Except(input.AllowedScopes);
                foreach (var scope in toBeRemoved)
                {
                    client.RemoveScope(scope);
                }

                var toBeAdd = input.AllowedScopes.Except(oldList);
                foreach (var scope in toBeAdd)
                {
                    client.AddScope(scope);
                }
            }

            if (input.AllowedGrantTypes != null)
            {
                var oldList =
                    ObjectMapper.Map<List<ClientGrantType>, List<string>>(client.AllowedGrantTypes);

                var toBeRemoved = oldList.Except(input.AllowedGrantTypes);
                foreach (var grant in toBeRemoved)
                {
                    client.RemoveGrantType(grant);
                }

                var toBeAdd = input.AllowedGrantTypes.Except(oldList);
                foreach (var grant in toBeAdd)
                {
                    client.AddGrantType(grant);
                }
            }

            if (input.AllowedCorsOrigins != null)
            {
                var oldList =
                    ObjectMapper.Map<List<ClientCorsOrigin>, List<string>>(
                        client.AllowedCorsOrigins);

                var toBeRemoved = oldList.Except(input.AllowedCorsOrigins);
                foreach (var cors in toBeRemoved)
                {
                    client.RemoveCorsOrigin(cors);
                }

                var toBeAdd = input.AllowedCorsOrigins.Except(oldList);
                foreach (var cors in toBeAdd)
                {
                    client.AddCorsOrigin(cors);
                }
            }

            if (input.RedirectUris != null)
            {
                var oldList =
                    ObjectMapper.Map<List<ClientRedirectUri>, List<string>>(client.RedirectUris);

                var toBeRemoved = oldList.Except(input.RedirectUris);
                foreach (var uri in toBeRemoved)
                {
                    client.RemoveRedirectUri(uri);
                }

                var toBeAdd = input.RedirectUris.Except(oldList);
                foreach (var uri in toBeAdd)
                {
                    client.AddRedirectUri(uri);
                }
            }

            if (input.PostLogoutRedirectUris != null)
            {
                var oldList =
                    ObjectMapper.Map<List<ClientPostLogoutRedirectUri>, List<string>>(
                        client.PostLogoutRedirectUris);

                var toBeRemoved = oldList.Except(input.PostLogoutRedirectUris);
                foreach (var uri in toBeRemoved)
                {
                    client.RemovePostLogoutRedirectUri(uri);
                }

                var toBeAdd = input.PostLogoutRedirectUris.Except(oldList);
                foreach (var uri in toBeAdd)
                {
                    client.AddPostLogoutRedirectUri(uri);
                }
            }

            #endregion

            if (input.Permissions != null)
            {
                var currentPermissions =
                    await _permissionManager.GetAllAsync(ClientPermissionValueProvider.ProviderName,
                        client.ClientId);
                var oldList = currentPermissions
                    .Where(permission => permission.IsGranted)
                    .Select(permission => permission.Name)
                    .ToList();

                var toBeRemoved = oldList.Except(input.Permissions);
                foreach (var permission in toBeRemoved)
                {
                    await _permissionManager.SetAsync(permission,
                        ClientPermissionValueProvider.ProviderName,
                        client.ClientId, false);
                }

                var toBeAdd = input.Permissions.Except(oldList);
                foreach (var permission in toBeAdd)
                {
                    await _permissionManager.SetAsync(permission,
                        ClientPermissionValueProvider.ProviderName,
                        client.ClientId, true);
                }
            }

            client = await _clientRepository.UpdateAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        [Authorize(IdentityServerPermissions.Client.Delete)]
        public async Task<JsonResult> DeleteAsync(Guid id)
        {
            var client = _clientRepository.FindAsync(id);
            if (client == null)
            {
                throw new EntityNotFoundException(typeof(Client), id);
            }

            // TODO: 跟进官方进度，是否 return true
            await _clientRepository.DeleteAsync(id);

            return new JsonResult(new
            {
                StatusCodes = StatusCodes.Status200OK,
                Message = "Delete Successfully"
            });
        }
    }
}
