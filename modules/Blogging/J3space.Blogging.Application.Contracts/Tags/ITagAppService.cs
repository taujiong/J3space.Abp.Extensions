using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Blogging.Tags.Dto;
using Volo.Abp.Application.Services;

namespace J3space.Blogging.Tags
{
    public interface ITagAppService : IApplicationService
    {
        public Task<List<TagListDto>> GetListAsync();
    }
}