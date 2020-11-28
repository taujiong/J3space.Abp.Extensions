using System;
using J3space.Blogging.MongoDB;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.Posts
{
    public class MongoPostRepository : MongoDbRepository<IBloggingMongoDbContext, Post, Guid>, IPostRepository
    {
        public MongoPostRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }
    }
}