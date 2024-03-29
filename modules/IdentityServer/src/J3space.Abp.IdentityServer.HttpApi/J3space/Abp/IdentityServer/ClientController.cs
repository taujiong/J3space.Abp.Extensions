﻿using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.Clients;
using J3space.Abp.IdentityServer.Clients.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace J3space.Abp.IdentityServer
{
    [RemoteService(Name = IdentityServerRemoteServiceConstants.RemoteServiceName)]
    [Area("identityServer")]
    [Route("api/ids/clients")]
    public class ClientController : IdentityServerControllerBase, IClientAppService
    {
        private readonly IClientAppService _clientAppService;

        public ClientController(IClientAppService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<ClientDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _clientAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public virtual Task<ClientDto> GetAsync(Guid id)
        {
            return _clientAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<ClientDto> CreateAsync(ClientCreateDto input)
        {
            return _clientAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public virtual Task<ClientDto> UpdateAsync(Guid id, ClientUpdateDto input)
        {
            return _clientAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _clientAppService.DeleteAsync(id);
        }
    }
}