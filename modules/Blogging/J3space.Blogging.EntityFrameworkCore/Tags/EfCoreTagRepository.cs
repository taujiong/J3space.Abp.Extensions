﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using J3space.Blogging.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace J3space.Blogging.Tags
{
    public class EfCoreTagRepository : EfCoreRepository<IBloggingDbContext, Tag, Guid>, ITagRepository
    {
        public EfCoreTagRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Tag>> GetListAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task DecreaseUsageCountOfTagsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var tags = await DbSet
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var tag in tags)
            {
                tag.DecreaseUsageCount();
                if (tag.UsageCount <= 0)
                {
                    await DeleteAsync(tag.Id, cancellationToken: GetCancellationToken(cancellationToken));
                }
            }
        }

        public async Task<Tag> FindByNameAsync(string tagName)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.Name == tagName);
        }
    }
}