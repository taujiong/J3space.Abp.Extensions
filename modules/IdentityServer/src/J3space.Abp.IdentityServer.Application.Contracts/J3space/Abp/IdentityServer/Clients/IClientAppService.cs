using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.Clients.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace J3space.Abp.IdentityServer.Clients
{
    public interface IClientAppService : IApplicationService
    {
        public Task<PagedResultDto<ClientDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        public Task<ClientDto> GetAsync(Guid id);
        public Task<ClientDto> CreateAsync(ClientCreateDto input);
        public Task<ClientDto> UpdateAsync(Guid id, ClientUpdateDto input);
        public Task DeleteAsync(Guid id);
    }
}