using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace J3space.Abp.IdentityServer.Clients
{
    public interface IClientAppService : IApplicationService
    {
        public Task<PagedResultDto<ClientDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        public Task<ClientDto> GetAsync(Guid id);
        public Task<ClientDto> CreateAsync(ClientCreateUpdateDto input);
        public Task<ClientDto> UpdateAsync(Guid id, ClientCreateUpdateDto input);
        public Task DeleteAsync(Guid id);
    }
}