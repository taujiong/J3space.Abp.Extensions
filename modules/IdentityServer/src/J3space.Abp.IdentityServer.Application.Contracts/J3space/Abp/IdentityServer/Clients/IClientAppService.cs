using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.Clients.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace J3space.Abp.IdentityServer.Clients
{
    public interface IClientAppService : IApplicationService
    {
        public Task<PagedResultDto<ClientGetUpdateDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        public Task<ClientGetUpdateDto> GetAsync(Guid id);
        public Task<ClientGetUpdateDto> CreateAsync(ClientCreateDto input);
        public Task<ClientGetUpdateDto> UpdateAsync(Guid id, ClientGetUpdateDto input);
        public Task DeleteAsync(Guid id);
    }
}