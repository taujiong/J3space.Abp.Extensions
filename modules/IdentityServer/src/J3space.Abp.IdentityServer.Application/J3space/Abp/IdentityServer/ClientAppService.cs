using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.Clients;
using J3space.Abp.IdentityServer.Clients.Dto;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.Clients;

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

        public virtual async Task<PagedResultDto<ClientGetUpdateDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var list = await _clientRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount);
            var totalCount = await _clientRepository.GetCountAsync();

            return new PagedResultDto<ClientGetUpdateDto>(
                totalCount,
                ObjectMapper.Map<List<Client>, List<ClientGetUpdateDto>>(list)
            );
        }

        public virtual async Task<ClientGetUpdateDto> GetAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);

            return ObjectMapper.Map<Client, ClientGetUpdateDto>(client);
        }

        [Authorize(IdentityServerPermissions.Client.Create)]
        public virtual async Task<ClientGetUpdateDto> CreateAsync(ClientCreateDto input)
        {
            var client = await _clientRepository.FindByCliendIdAsync(input.ClientId);
            if (client == null)
            {
                client = new Client(GuidGenerator.Create(), input.ClientId)
                {
                    ClientName = input.ClientId,
                    ProtocolType = "oidc"
                };
                await _clientRepository.InsertAsync(client, true);
            }

            return ObjectMapper.Map<Client, ClientGetUpdateDto>(client);
        }

        [Authorize(IdentityServerPermissions.Client.Update)]
        public virtual async Task<ClientGetUpdateDto> UpdateAsync(Guid id, ClientGetUpdateDto input)
        {
            var client = await _clientRepository.FindAsync(id);

            return ObjectMapper.Map<Client, ClientGetUpdateDto>(client);
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