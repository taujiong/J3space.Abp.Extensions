using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace J3space.Blogging.Tags
{
    public interface ITagRepository : IBasicRepository<Tag, Guid>
    {
        Task<List<Tag>> GetListAsync(CancellationToken cancellationToken = default);
        Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        Task DecreaseUsageCountOfTagsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
        Task<Tag> FindByNameAsync(string tagName, CancellationToken cancellationToken = default);
    }
}