using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.Clients;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace J3space.Abp.IdentityServer
{
    [RemoteService(Name = IdentityServerRemoteServiceConstants.RemoteServiceName)]
    [Area("identityServer")]
    [Route("api/ids/clients")]
    public class ClientController : IdentityServerController, IClientAppService
    {
        private readonly IClientAppService _clientAppService;

        public ClientController(IClientAppService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        [HttpGet]
        public Task<PagedResultDto<ClientDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _clientAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("all")]
        public Task<ListResultDto<ClientDto>> GetAllListAsync()
        {
            return _clientAppService.GetAllListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<ClientDto> GetAsync(Guid id)
        {
            return _clientAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<ClientDto> CreateAsync(ClientCreateUpdateDto input)
        {
            return _clientAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<ClientDto> UpdateAsync(Guid id, ClientCreateUpdateDto input)
        {
            return _clientAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public Task<JsonResult> DeleteAsync(Guid id)
        {
            return _clientAppService.DeleteAsync(id);
        }
    }
}
