using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using J3space.Blogging.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.Tags
{
    public class MongoTagRepository : MongoDbRepository<IBloggingMongoDbContext, Tag, Guid>, ITagRepository
    {
        public MongoTagRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public async Task<List<Tag>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task DecreaseUsageCountOfTagsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var tags = await (await GetMongoQueryableAsync(cancellationToken))
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var tag in tags)
            {
                tag.DecreaseUsageCount();
                await UpdateAsync(tag, cancellationToken: GetCancellationToken(cancellationToken));
            }
        }

        public async Task<Tag> FindByNameAsync(string tagName, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .FirstOrDefaultAsync(t => t.Name == tagName, GetCancellationToken(cancellationToken));
        }
    }
}