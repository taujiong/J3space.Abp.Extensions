using System;
using System.Linq;
using J3space.Blogging.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace J3space.Blogging.Posts
{
    public class EfCorePostRepository : EfCoreRepository<IBloggingDbContext, Post, Guid>, IPostRepository
    {
        public EfCorePostRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override IQueryable<Post> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}