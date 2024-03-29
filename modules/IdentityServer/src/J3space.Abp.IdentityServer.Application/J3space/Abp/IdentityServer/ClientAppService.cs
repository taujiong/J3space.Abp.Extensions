﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using J3space.Abp.IdentityServer.Clients;
using J3space.Abp.IdentityServer.Clients.Dto;
using J3space.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.Clients;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace J3space.Abp.IdentityServer
{
    [Authorize(IdentityServerPermissions.Client.Default)]
    public class ClientAppService : IdentityServerAppServiceBase, IClientAppService
    {
        private readonly IClientRepository _clientRepository;

        public ClientAppService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public virtual async Task<PagedResultDto<ClientDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var list = await _clientRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount, includeDetails: true);
            var totalCount = await _clientRepository.GetCountAsync();

            return new PagedResultDto<ClientDto>(
                totalCount,
                ObjectMapper.Map<List<Client>, List<ClientDto>>(list)
            );
        }

        public virtual async Task<ClientDto> GetAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        [Authorize(IdentityServerPermissions.Client.Create)]
        public virtual async Task<ClientDto> CreateAsync(ClientCreateDto input)
        {
            var clientExist = await _clientRepository.CheckClientIdExistAsync(input.ClientId);
            if (clientExist)
                throw new UserFriendlyException(L["EntityExisted", nameof(Client), nameof(Client.ClientId),
                    input.ClientId]);

            var client = new Client(GuidGenerator.Create(), input.ClientId)
            {
                ClientName = input.ClientName,
                ClientUri = input.ClientUri,
                LogoUri = input.LogoUri,
                Description = input.Description,
                RequireConsent = input.RequireConsent
            };

            input.AllowedScopes.ForEach(s => client.AddScope(s));
            input.RedirectUrls.ForEach(url => client.AddRedirectUri(url));
            input.PostLogoutUrls.ForEach(url => client.AddPostLogoutRedirectUri(url));
            input.ClientSecrets.ForEach(s =>
                client.AddSecret(s.Value.Sha256(), s.Expiration, description: s.Description));

            await _clientRepository.InsertAsync(client, true);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        [Authorize(IdentityServerPermissions.Client.Update)]
        public virtual async Task<ClientDto> UpdateAsync(Guid id, ClientUpdateDto input)
        {
            var client = await _clientRepository.GetAsync(id);

            var clientExist = await _clientRepository.CheckClientIdExistAsync(input.ClientId, id);
            if (clientExist)
                throw new UserFriendlyException(L["EntityExisted", nameof(Client), nameof(Client.ClientId),
                    input.ClientId]);

            client = ObjectMapper.Map(input, client);

            client.RemoveAllPostLogoutRedirectUris();
            input.PostLogoutRedirectUris
                .ForEach(x => client.AddPostLogoutRedirectUri(x));

            client.RemoveAllRedirectUris();
            input.RedirectUris
                .ForEach(x => client.AddRedirectUri(x));

            client.RemoveAllCorsOrigins();
            input.AllowedCorsOrigins
                .ForEach(x => client.AddCorsOrigin(x));

            client.RemoveAllAllowedGrantTypes();
            input.AllowedGrantTypes
                .ForEach(x => client.AddGrantType(x));

            client.RemoveAllScopes();
            input.AllowedScopes
                .ForEach(x => client.AddScope(x));

            client.ClientSecrets.Clear();
            input.ClientSecrets.ForEach(s =>
                client.AddSecret(s.Value.Sha256(), s.Expiration, description: s.Description));

            client.RemoveAllClaims();
            input.Claims.ForEach(c => client.AddClaim(c.Value, c.Type));

            client.RemoveAllProperties();
            input.Properties.ForEach(p => client.AddProperty(p.Key, p.Value));

            client = await _clientRepository.UpdateAsync(client);
            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        [Authorize(IdentityServerPermissions.Client.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var client = await _clientRepository.FindAsync(id);
            if (client == null) throw new EntityNotFoundException(typeof(Client), id);

            await _clientRepository.DeleteAsync(id);
        }
    }
}